using System.Collections.Generic;
using System.Linq;
using ShoppingBasket.Interfaces;

namespace ShoppingBasket.Discounts
{
    public class AllItemsDiscount : Discount
    {
        private readonly int percentageDiscount;

        public AllItemsDiscount(IEnumerable<IBasketItem> basketItems, string code, string name, int percentageDiscount)
        {
            this.percentageDiscount = percentageDiscount;
            Code = code;
            Name = name;
            BasketItems = basketItems;
        }

        public override long GetTotalDiscountInPence()
        {
            if (BasketItems != null)
            {
                return BasketItems.Sum(basketItem =>
                        basketItem.Quantity * basketItem.Product.GetPriceInPence() * percentageDiscount / 100);
            }

            return 0;
        }

        public override int GetNoOfMatchedInstances()
        {
            return 1;
        }
    }
}