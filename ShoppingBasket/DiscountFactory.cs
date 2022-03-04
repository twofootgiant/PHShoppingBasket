using System.Collections.Generic;
using ShoppingBasket.Discounts;
using ShoppingBasket.Interfaces;

namespace ShoppingBasket
{
    public static class DiscountFactory
    {
        public static IDiscount Create(string discountCode, IEnumerable<IBasketItem> basketItems)
        {
            // Improvement: In a real world scenario, this section would be driven from config files and/or dependency injection e.g. Unity
            Discount discount = null;
            switch (discountCode)
            {
                case "APPLE241":
                    discount = new BogofDiscount(basketItems, discountCode, "2 for the price of 1 on apples", "f1"); 
                    break;
                case "MD1":
                    discount = new CategoryCombinationDiscount(basketItems, discountCode, "Meal deal", new[] { 'f', 'd', 's' }, 25);
                    break;
                case "BDD":
                    discount = new AllItemsDiscount(basketItems, discountCode, "Bargain discount day", 10);
                    break;
            }

            return discount;
        }
    }
}
