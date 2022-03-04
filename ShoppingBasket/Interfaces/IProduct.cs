using System;

namespace ShoppingBasket
{
    /**
    * Models a single product available for purchase.
    */
    public interface IProduct : IEquatable<IProduct>
    {
        /**
         * Unique identifier of product
         * @return (not null).
         */
        string GetCode();

        /**
         * Display name of product
         * @return
         */
        string GetName();

        /**
         * Price of product in penny sterling.
         * @return
         */
        int GetPriceInPence();

        /*
         * Product type
         */
        char Category { get; }
    }
}