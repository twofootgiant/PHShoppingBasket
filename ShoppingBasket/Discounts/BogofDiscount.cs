using System.Collections.Generic;
using System.Linq;
using ShoppingBasket.Interfaces;

namespace ShoppingBasket.Discounts
{
    public class BogofDiscount : Discount
    {
        private readonly string productCodeOfInterest;

        public BogofDiscount(IEnumerable<IBasketItem> basketItems, string code, string name, string productCodeOfInterest)
        {
            Code = code;
            Name = name;
            BasketItems = basketItems;
            this.productCodeOfInterest = productCodeOfInterest;
        }

        public override long GetTotalDiscountInPence()
        {
            return GetRelevantBasketItems().Select(basketItem => basketItem.Quantity * basketItem.Product.GetPriceInPence() / 2).FirstOrDefault();
        }

        public override int GetNoOfMatchedInstances()
        {
            return GetRelevantBasketItems().Select(basketItem => basketItem.Quantity / 2).FirstOrDefault();
        }

        private IEnumerable<IBasketItem> GetRelevantBasketItems()
        {
            // Improvement: This is called twice to create the receipt line. This is wasteful.
            // It maybe better to use an Observable collection to enable the caching and updating of this filter
            if (BasketItems != null)
            {
                return BasketItems.Where(basketItem => basketItem.Quantity >= 2)
                    .Where(basketItem => string.Equals(basketItem.Product.GetCode(), productCodeOfInterest));
            }

            return new List<IBasketItem>();
        }
    }
}