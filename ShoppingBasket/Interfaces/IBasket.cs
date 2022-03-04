namespace ShoppingBasket.Interfaces
{
    /**
     * Models a single shopping basket containing products, quantities and discounts.
     */
    interface IBasket
    {
        /**
         * Adds a new product to the basket, incrementing quantity if already present.
         * @param product (not null).
         * @return new quantity of product in basket.
         */
        int AddProduct(IProduct product);

        /**
         * Decrements the quantity of a product in the basket and removes the product entirely if new quantity is 0.
         * @param product (not null).
         * @return new quantity of product in basket.
         */
        int RemoveProduct(IProduct product);

        /**
         * Updates the quantity of a product in the basket.  If quantity is 0 then product is removed entirely from basket.
         * @param product (not null).
         * @param quantity new quantity of product (quantity >= 0).
         * @return true if basket state changed, otherwise false.
         */
        bool UpdateQuantity(IProduct product, int quantity);

        /**
         * Gets the quantity of a product in the basket.
         * @param product (not null).
         * @return current quantity in basket
         */
        int GetQuantity(IProduct product);

        /**
         * Clears tbe basket of all products.
         * @return true if basket state changed, otherwise false.
         */
        bool Clear();

        /**
         * Total quantity of all products in the basket.
         * @return (>= 0)
         */
        long GetTotalQuantity();

        /**
         * Total cost of all products in the basket. Sum of (price x quantity) per product.
         * @return (>= 0)
         */
        long GetTotalCostInPence();

        /**
         * Total discounts across all discount codes applied to the basket.
         * @return (>= 0)
         */
        long GetTotalDiscountsInPence();

        /**
         * Creates a new receipt of the basket contents.
         * @return (not null)
         */
        string GenerateReceipt();
        
        /**
         * Clears tbe basket of all discount codes.
         * @return true if basket state changed, otherwise false.
         */
        bool ClearDiscountCodes();
    }
}
