using System;

namespace ShoppingBasket
{
    class Program
    {
        static void Main(string[] args)
        {
            var basket = new Basket();
            var product1 = Product.Create("f1", "Apple", 50, out _);
            basket.AddProduct(product1);
            basket.AddProduct(product1);
            basket.AddProduct(product1);
            basket.AddProduct(product1);

            var product2 = Product.Create("f3", "Pear", 40, out _);
            basket.AddProduct(product2);

            basket.AddDiscount("APPLE241");
            basket.AddDiscount("BDD");

            Console.WriteLine(basket.GenerateReceipt());
            Console.ReadLine();
        }
    }
}
