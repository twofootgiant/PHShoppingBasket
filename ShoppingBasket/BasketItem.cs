using ShoppingBasket.Interfaces;

namespace ShoppingBasket
{
    public class BasketItem : IBasketItem
    {
        public BasketItem(IProduct product, int quantity = 1)
        {
            Product = product;
            Quantity = quantity;
        }

        public int Quantity { get; private set; }

        public IProduct Product { get; }

        public int IncrementQuantity(int increment = 1)
        {
            return Quantity += increment;
        }

        public int DecrementQuantity(int decrement = 1)
        {
            return Quantity -= decrement;
        }

        public int SetQuantity(int quantity)
        {
            Quantity = quantity;
            return Quantity;
        }

        public bool Equals(IBasketItem other)
        {
            return other != null && Equals(Product, other.Product);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((IBasketItem)obj);
        }

        public override int GetHashCode()
        {
            return (Product != null ? Product.GetHashCode() : 0);
        }
    }
}