using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShoppingBasket.Interfaces;

namespace ShoppingBasket
{
    public class Basket : IBasket
    {
        // Improvement: To make this thread safe the concurrentdictionary can be used
        private readonly HashSet<IBasketItem> basketItems;
        private readonly Dictionary<string, IDiscount> discounts;

        public Basket()
        {
            basketItems = new HashSet<IBasketItem>();
            discounts = new Dictionary<string, IDiscount>();
        }

        public int AddProduct(IProduct product)
        {
            return AddProduct(product, 1);
        }

        // Improvement: To make this thread safe the Interlocked methods can be used
        public int AddProduct(IProduct product, int quantity)
        {
            var basketItem = GetExistingBasketItem(product);

            if (basketItem != null)
            {
                basketItem.IncrementQuantity(quantity);
            }
            else
            {
                basketItem = new BasketItem(product, quantity);
                basketItems.Add(basketItem);
            }

            return basketItem.Quantity;
        }

        private IBasketItem GetExistingBasketItem(IProduct product)
        {
            return basketItems.FirstOrDefault(x => x.Product.GetCode() == product.GetCode());
        }

        // Improvement: To make this thread safe the Interlocked methods can be used
        public int RemoveProduct(IProduct product)
        {
            // Improvement : Add logging here or some other means to state that no product was removed e.g. log4Net
            var basketItem = GetExistingBasketItem(product);

            if (basketItem == null) return 0;

            if (basketItem.Quantity <= 1)
            {
                basketItems.Remove(basketItem);
            }
            else
            {
                basketItem.DecrementQuantity();
            }

            return basketItem.Quantity;
        }

        public bool UpdateQuantity(IProduct product, int quantity)
        {
            if (quantity < 0) return false;

            var basketItem = GetExistingBasketItem(product);

            if (basketItem != null)
            {
                if (basketItem.Quantity == quantity)
                {
                    return false;
                }

                if (quantity == 0)
                {
                    RemoveProduct(product);
                }
                else
                {
                    basketItem.SetQuantity(quantity);
                }
            }
            else if (quantity != 0)
            {
                // Assumption : We are allowing this to be used as a proxy for adding a new product
                AddProduct(product, quantity);
            }

            return true;
        }

        public int GetQuantity(IProduct product)
        {
            var basketItem = GetExistingBasketItem(product);
            return basketItem?.Quantity ?? 0;
        }

        public bool Clear()
        {
            if (basketItems.Count <= 0) return false;

            basketItems.Clear();
            return true;
        }

        public bool ClearDiscountCodes()
        {
            if (discounts.Count <= 0) return false;

            discounts.Clear();
            return true;
        }

        public long GetTotalQuantity()
        {
            return basketItems.Sum(b => b.Quantity);
        }

        public int AddDiscount(string discountCode)
        {
            if (discounts.ContainsKey(discountCode)) return discounts.Count;
            var discount = DiscountFactory.Create(discountCode, basketItems);

            if (discount != null)
            {
                discounts.Add(discountCode, discount);
            }

            return discounts.Count;
        }

        public long GetTotalCostInPence()
        {
            return basketItems.Aggregate(0L,
                (current, basketItem) => current + basketItem.Product.GetPriceInPence() * basketItem.Quantity);
        }

        public long GetTotalDiscountsInPence()
        {
            // Note: This design requires that each discount object iterate through the list of basket items.
            // This would be inefficient if lots of discounts need to be applied or if the basket size was really large
            // or if the total cost is required in anything like real time. In those cases, an event driven approach may be more appropriate.
            // Each discount object would listen for changes to the basket collection (maybe using an ObservableCollection or similar)
            // and recalculate it's discount value as needed.

            // Improvement : At present all the discount types are independent of each other
            // (i.e. there is no discount type that is only applicable after some or all other discounts have been applied).
            // This means that they can all run in parallel (multi threaded) and still produce the same result
            // If this requirement were to change, this method would have be more intelligent, maybe requiring the creation of a
            // dedicated discount processor class
            return discounts.AsParallel().Aggregate(0L,
                (current, discount) =>
                    current + discount.Value.GetTotalDiscountInPence());
        }
        
        public string GenerateReceipt()
        {
            // Improvement: the currency formatting in this method can be extended to other localities by using CultureInfo at a later date if required
            if (basketItems.Count < 1) return string.Empty;

            var receiptText = new StringBuilder();
            var total = 0L;
            total = AddItemLines(receiptText, total);
            var totalInPoundsAndPence = (decimal)total / 100;
            receiptText.AppendLine($"Sub-total = {totalInPoundsAndPence:C2}");

            var discountsSubTotal = 0L;
            discountsSubTotal = AddDiscountLines(receiptText, discountsSubTotal);

            receiptText.AppendLine(string.Empty);
            receiptText.AppendLine("=== Total ===");
            totalInPoundsAndPence = (decimal)(total - discountsSubTotal) / 100;
            receiptText.AppendLine($"{totalInPoundsAndPence:C2}p");

            return receiptText.ToString();
        }

        private long AddItemLines(StringBuilder receiptText, long total)
        {
            receiptText.AppendLine("=== Items ===");
            foreach (var basketItem in basketItems)
            {
                var priceInPence = basketItem.Product.GetPriceInPence();

                var subtotalInPence = priceInPence * basketItem.Quantity;
                total += subtotalInPence;

                var displayedPrice = priceInPence < 100 ? $"{priceInPence}p" : $"{(decimal)priceInPence / 100:C2}";

                var subtotalInPoundsAndPence = (decimal)subtotalInPence / 100;
                receiptText.AppendLine(
                    $"{basketItem.Quantity}x {basketItem.Product.GetName()} @ {displayedPrice} = {subtotalInPoundsAndPence:C2}");
            }

            return total;
        }

        private long AddDiscountLines(StringBuilder receiptText, long subtotalInPence)
        {
            if (discounts.Count <= 0) return subtotalInPence;

            receiptText.AppendLine(string.Empty);
            receiptText.AppendLine("=== Discounts ===");
            subtotalInPence = 0;
            foreach (var discount in discounts.Values)
            {
                var discountTotalInPence = discount.GetTotalDiscountInPence();
                var discountTotalInPoundsAndPence = (decimal) discountTotalInPence / 100;

                receiptText.AppendLine(
                    $"{discount.GetNoOfMatchedInstances()}x {discount.GetCode()} = -{discountTotalInPoundsAndPence:C2}");

                subtotalInPence += discountTotalInPence;
            }
            var subtotalInPoundsAndPence = (decimal)subtotalInPence / 100;
            receiptText.AppendLine($"Sub-total = -{subtotalInPoundsAndPence:C2}");

            return subtotalInPence;
        }
    }
}
