using System;

namespace ShoppingBasket.Interfaces
{    
    /**
     * Models a single shopping basket item containing product and quantity.
     */
    public interface IBasketItem : IEquatable<IBasketItem>
    {
        int Quantity { get; }

        IProduct Product { get; }

        int SetQuantity(int quantity);

        int IncrementQuantity(int increment = 1);

        int DecrementQuantity(int decrement = 1);
    }
}