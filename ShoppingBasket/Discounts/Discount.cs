using System.Collections.Generic;
using ShoppingBasket.Interfaces;

namespace ShoppingBasket.Discounts
{
    public abstract class Discount : IDiscount
    {
        protected IEnumerable<IBasketItem> BasketItems;
        protected string Code;
        protected string Name;
        
        public bool Equals(IDiscount other)
        {
            return other != null && GetCode().Equals(other.GetCode());
        }

        public string GetCode()
        {
            // Improvement: This would be better as a readonly property
            return Code;
        }

        public string GetName()
        {
            // Improvement: This would be better as a readonly property
            return Name;
        }

        public abstract long GetTotalDiscountInPence();
        public abstract int GetNoOfMatchedInstances();
    }
}