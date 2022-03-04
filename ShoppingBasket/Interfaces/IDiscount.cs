using System;

namespace ShoppingBasket.Interfaces
{
    public interface IDiscount : IEquatable<IDiscount>
    {
        /**
         * Unique identifier of discount.
         * @return (not null).
         */
        string GetCode();

        /*
         * Display name of the discount.
         * @return (not null).
         */
        string GetName();

        /*
         * The total amount in pence to be subtracted on account of this discount code
         * @return 0
         */
        long GetTotalDiscountInPence();

        /*
         * No of times this discount will be applied to the basket
         */
        int GetNoOfMatchedInstances();
    }
}
