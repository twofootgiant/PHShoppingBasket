using NUnit.Framework;
using ShoppingBasket;

namespace ShoppingBasketTests
{
    [TestFixture]
    public class BasketTest
    {
        [Test]
        public void Constructor_CreatesEmptyBasket()
        {
            var basket = new Basket();
            Assert.AreEqual(0, basket.GetTotalQuantity());
        }

        [Test]
        public void AddProduct_SingleProduct_AddsASingleItem()
        {
            var basket = new Basket();
            var product = Product.Create("f1", "Apple", 50, out _);
            basket.AddProduct(product);

            Assert.AreEqual(1, basket.GetTotalQuantity());
        }

        [Test]
        public void AddProduct_TwoProducts_Adds2Items()
        {
            var basket = new Basket();
            
            var product1 = Product.Create("f1", "Apple", 50, out _);
            var product2 = Product.Create("f2", "Banana", 30, out _);
            basket.AddProduct(product1);
            basket.AddProduct(product2);

            Assert.AreEqual(2, basket.GetTotalQuantity());
        }

        [Test]
        public void AddProduct_MultipleOfTheSameProducts_AddsMultipleOfTheSameProducts()
        {
            var basket = new Basket();
            var product1 = Product.Create("f1", "Apple", 50, out  _);
            var product2 = Product.Create("f2", "Banana", 30, out  _);
            basket.AddProduct(product1);
            basket.AddProduct(product2);
            basket.AddProduct(product1);
            basket.AddProduct(product2);
            basket.AddProduct(product2);

            Assert.AreEqual(2, basket.GetQuantity(product1));
            Assert.AreEqual(3, basket.GetQuantity(product2));
            Assert.AreEqual(5, basket.GetTotalQuantity());
        }

        [Test]
        public void RemoveProduct_ExistingProduct_RemovesOneItem()
        {
            var basket = new Basket();
            var product1 = Product.Create("f1", "Apple", 50, out _);
            basket.AddProduct(product1);
            var product1InBasketCount = basket.AddProduct(product1);

            Assert.AreEqual(2, product1InBasketCount);
            Assert.AreEqual(product1InBasketCount, basket.GetQuantity(product1));
            Assert.AreEqual(2, basket.GetTotalQuantity());

            product1InBasketCount =  basket.RemoveProduct(product1);

            Assert.AreEqual(product1InBasketCount, basket.GetQuantity(product1));
            Assert.AreEqual(1, basket.GetTotalQuantity());
        }

        [Test]
        public void RemoveProduct_ProductNotInBasket_RemovesNoItems()
        {
            var basket = new Basket();
            var product1 = Product.Create("f1", "Apple", 50, out _);
            basket.AddProduct(product1);

            Assert.AreEqual(1, basket.GetTotalQuantity());

            var product2 = Product.Create("f2", "Banana", 30, out _);
            var product2InBasketCount = basket.RemoveProduct(product2);

            Assert.AreEqual(0, product2InBasketCount);
            Assert.AreEqual(1, basket.GetTotalQuantity());
        }

        [Test]
        public void RemoveProduct_LastItemOfExistingProduct_RemovesItem()
        {
            var basket = new Basket();
            var product1 = Product.Create("f1", "Apple", 50, out _);
            basket.AddProduct(product1);

            Assert.AreEqual(1, basket.GetQuantity(product1));

            basket.RemoveProduct(product1);

            Assert.AreEqual(0, basket.GetQuantity(product1));
            Assert.AreEqual(0, basket.GetTotalQuantity());
        }

        [Test]
        public void UpdateQuantity_OfAnExistingItem_UpdatesTheQuantityCount()
        {
            var basket = new Basket();
            var product1 = Product.Create("f1", "Apple", 50, out _);
            basket.AddProduct(product1);

            Assert.AreEqual(1, basket.GetQuantity(product1));

            var result = basket.UpdateQuantity(product1, 15);

            Assert.IsTrue(result);

            Assert.AreEqual(15, basket.GetQuantity(product1));
        }

        [Test]
        public void UpdateQuantity_OfProductNotInBasket_AddsProductWithTheQuantityCount()
        {
            var basket = new Basket();
            var product1 = Product.Create("f1", "Apple", 50, out _);

            Assert.AreEqual(0, basket.GetQuantity(product1));

            var result = basket.UpdateQuantity(product1, 15);

            Assert.IsTrue(result);

            Assert.AreEqual(15, basket.GetQuantity(product1));
        }

        [Test]
        public void UpdateQuantity_NegativeQuantityOfAnExistingItem_FailsToUpdateQuantity()
        {
            var basket = new Basket();
            var product1 = Product.Create("f1", "Apple", 50, out _);
            basket.AddProduct(product1);

            Assert.AreEqual(1, basket.GetQuantity(product1));

            var result = basket.UpdateQuantity(product1, -15);

            Assert.IsFalse(result);

            Assert.AreEqual(1, basket.GetQuantity(product1));
        }

        [Test]
        public void UpdateQuantity_NegativeQuantityOfProductNotInBasket_FailsToAddProductWithTheQuantityCount()
        {
            var basket = new Basket();
            var product1 = Product.Create("f1", "Apple", 50, out _);

            Assert.AreEqual(0, basket.GetQuantity(product1));

            var result = basket.UpdateQuantity(product1, -15);

            Assert.IsFalse(result);

            Assert.AreEqual(0, basket.GetQuantity(product1));
        }

        [Test]
        public void UpdateQuantity_ZeroQuantityOfAnExistingItem_RemovesItemFromBasket()
        {
            var basket = new Basket();
            var product1 = Product.Create("f1", "Apple", 50, out _);
            basket.AddProduct(product1);

            Assert.AreEqual(1, basket.GetQuantity(product1));

            var result = basket.UpdateQuantity(product1, 0);

            Assert.IsTrue(result);

            Assert.AreEqual(0, basket.GetQuantity(product1));
        }

        [Test]
        public void UpdateQuantity_ZeroQuantityOfProductNotInBasket_DoesNotAddProductToBasket()
        {
            var basket = new Basket();
            var product1 = Product.Create("f1", "Apple", 50, out _);

            Assert.AreEqual(0, basket.GetQuantity(product1));

            var result = basket.UpdateQuantity(product1, 0);

            // Note: this is technically accurate as zero was requested and zero was "added"
            Assert.IsTrue(result);

            Assert.AreEqual(0, basket.GetQuantity(product1));
        }

        [Test]
        public void GetTotalCostInPence_SingleItem_ReturnsPriceForSingleItem()
        {
            var basket = new Basket();
            var product1 = Product.Create("f1", "Apple", 50, out _);
            basket.AddProduct(product1);

            Assert.AreEqual(1, basket.GetQuantity(product1));

            Assert.AreEqual(product1.GetPriceInPence(), basket.GetTotalCostInPence());
        }

        [Test]
        public void GetTotalCostInPence_MultipleOfTheSameItem_ReturnsPriceForSingleItem()
        {
            var basket = new Basket();
            var product1 = Product.Create("f1", "Apple", 50, out _);
            basket.AddProduct(product1);
            basket.AddProduct(product1);

            Assert.AreEqual(2, basket.GetQuantity(product1));

            Assert.AreEqual(product1.GetPriceInPence() * 2, basket.GetTotalCostInPence());

        }

        [Test]
        public void GetTotalCostInPence_MultipleOfDifferentItems_ReturnsPriceForSingleItem()
        {

            var basket = new Basket();
            var product1 = Product.Create("f1", "Apple", 50, out _);
            basket.AddProduct(product1);
            basket.AddProduct(product1);

            var product2 = Product.Create("f2", "Banana", 30, out _);
            basket.AddProduct(product2);
            basket.AddProduct(product2);
            basket.AddProduct(product2);

            Assert.AreEqual(2, basket.GetQuantity(product1));
            Assert.AreEqual(3, basket.GetQuantity(product2));

            var expectedTotalInPence = product1.GetPriceInPence() * 2 + product2.GetPriceInPence() * 3;

            Assert.AreEqual(expectedTotalInPence, basket.GetTotalCostInPence());
        }

        [Test]
        public void GenerateReceipt_MultipleOfDifferentItems_ReturnsItemisedLinesForReceipt()
        {

            var basket = new Basket();
            var product1 = Product.Create("f1", "Apple", 50, out _);
            basket.AddProduct(product1);

            var product2 = Product.Create("f2", "Banana", 30, out _);
            basket.AddProduct(product2);
            basket.AddProduct(product2);

            var product3 = Product.Create("d1", "Cola", 100, out _);
            basket.AddProduct(product3);
            basket.AddProduct(product3);
            basket.AddProduct(product3);

            Assert.AreEqual(1, basket.GetQuantity(product1));
            Assert.AreEqual(2, basket.GetQuantity(product2));
            Assert.AreEqual(3, basket.GetQuantity(product3));

            var expectedReceipt = "=== Items ===\r\n1x Apple @ 50p = £0.50\r\n2x Banana @ 30p = £0.60\r\n3x Cola @ £1.00 = £3.00\r\nSub-total = £4.10\r\n\r\n=== Total ===\r\n£4.10p\r\n";

            Assert.AreEqual(expectedReceipt, basket.GenerateReceipt());
        }

        [Test]
        public void GetTotalCostInPence_DiscountCodeAPPLE241With1Apple_NoDiscountApplied()
        {
            var basket = new Basket();
            var product1 = Product.Create("f1", "Apple", 50, out _);
            basket.AddProduct(product1);
            var product2 = Product.Create("f2", "Banana", 30, out _);
            basket.AddProduct(product2);

            basket.AddDiscount("APPLE241");

            Assert.AreEqual(1, basket.GetQuantity(product1));
            Assert.AreEqual(1, basket.GetQuantity(product2));

            Assert.AreEqual(product1.GetPriceInPence() + product2.GetPriceInPence(), basket.GetTotalCostInPence());
        }

        [Test]
        public void GetTotalCostInPence_DiscountCodeAPPLE241With2Apples_DiscountAppliedOnce()
        {
            var basket = new Basket();
            var product1 = Product.Create("f1", "Apple", 50, out _);
            basket.AddProduct(product1);
            basket.AddProduct(product1);

            basket.AddDiscount("APPLE241");

            Assert.AreEqual(2, basket.GetQuantity(product1));

            Assert.AreEqual(product1.GetPriceInPence() * 2 * 0.5, basket.GetTotalCostInPence() - basket.GetTotalDiscountsInPence());
        }

        [Test]
        public void GetTotalCostInPence_DiscountCodeAPPLE241With4Apples_DiscountAppliedTwice()
        {
            var basket = new Basket();
            var product1 = Product.Create("f1", "Apple", 50, out _);
            basket.AddProduct(product1);
            basket.AddProduct(product1);
            basket.AddProduct(product1);
            basket.AddProduct(product1);

            basket.AddDiscount("APPLE241");

            Assert.AreEqual(4, basket.GetQuantity(product1));

            Assert.AreEqual(product1.GetPriceInPence() * 4 * 0.5, basket.GetTotalCostInPence() - basket.GetTotalDiscountsInPence());
        }

        [Test]
        public void GetTotalCostInPence_DiscountCodeMD1With2Items_NoDiscountApplied()
        {
            var basket = new Basket();
            var product1 = Product.Create("f1", "Apple", 50, out _);
            basket.AddProduct(product1);
            var product2 = Product.Create("d1", "Cola", 100, out _);
            basket.AddProduct(product2);

            basket.AddDiscount("MD1");

            Assert.AreEqual(1, basket.GetQuantity(product1));
            Assert.AreEqual(1, basket.GetQuantity(product2));

            Assert.AreEqual(product1.GetPriceInPence() + product2.GetPriceInPence(), basket.GetTotalCostInPence());
        }

        [Test]
        public void GetTotalCostInPence_DiscountCodeMD1With3Items_DiscountApplied()
        {
            var basket = new Basket();
            var product1 = Product.Create("f1", "Apple", 50, out _);
            basket.AddProduct(product1);
            var product2 = Product.Create("d1", "Cola", 100, out _);
            basket.AddProduct(product2);
            var product3 = Product.Create("s1", "Cheese sandwich", 150, out _);
            basket.AddProduct(product3);

            basket.AddDiscount("MD1");

            Assert.AreEqual(1, basket.GetQuantity(product1));
            Assert.AreEqual(1, basket.GetQuantity(product2));
            Assert.AreEqual(1, basket.GetQuantity(product3));
            var expected = (product1.GetPriceInPence() + product2.GetPriceInPence() + product3.GetPriceInPence()) * 0.75;
            Assert.AreEqual(expected, basket.GetTotalCostInPence() - basket.GetTotalDiscountsInPence());
        }

        [Test]
        public void GetTotalCostInPence_DiscountCodeMD1With7Items_DiscountAppliedTwice()
        {
            var basket = new Basket();
            var product1 = Product.Create("f1", "Apple", 50, out _);
            basket.AddProduct(product1);
            basket.AddProduct(product1);
            var product2 = Product.Create("d1", "Cola", 100, out _);
            basket.AddProduct(product2);
            basket.AddProduct(product2);
            var product3 = Product.Create("s1", "Cheese sandwich", 150, out _);
            basket.AddProduct(product3);
            basket.AddProduct(product3);
            var product4 = Product.Create("s2", "Chicken wrap", 200, out _);
            basket.AddProduct(product4);

            basket.AddDiscount("MD1");

            Assert.AreEqual(2, basket.GetQuantity(product1));
            Assert.AreEqual(2, basket.GetQuantity(product2));
            Assert.AreEqual(2, basket.GetQuantity(product3));
            Assert.AreEqual(1, basket.GetQuantity(product4));
            var expected = (product1.GetPriceInPence() + product2.GetPriceInPence() + product3.GetPriceInPence()) * 2 * 0.75 + product4.GetPriceInPence();
            Assert.AreEqual(expected, basket.GetTotalCostInPence() - basket.GetTotalDiscountsInPence());
        }

        [Test]
        public void GetTotalCostInPence_2DiscountCodeMD1AndAPPLE241With7Items_MD1AppliedTwiceAndAPPLE241AppliedOnce()
        {
            var basket = new Basket();
            var product1 = Product.Create("f1", "Apple", 50, out _);
            basket.AddProduct(product1);
            basket.AddProduct(product1);
            var product2 = Product.Create("d1", "Cola", 100, out _);
            basket.AddProduct(product2);
            basket.AddProduct(product2);
            var product3 = Product.Create("s1", "Cheese sandwich", 150, out _);
            basket.AddProduct(product3);
            basket.AddProduct(product3);
            basket.AddProduct(product3);

            basket.AddDiscount("MD1");
            basket.AddDiscount("APPLE241");

            Assert.AreEqual(2, basket.GetQuantity(product1));
            Assert.AreEqual(2, basket.GetQuantity(product2));
            Assert.AreEqual(3, basket.GetQuantity(product3));
            var expected = (product1.GetPriceInPence() + product2.GetPriceInPence() + product3.GetPriceInPence()) * 2 * 0.75 + product3.GetPriceInPence() - product1.GetPriceInPence();
            Assert.AreEqual(expected, basket.GetTotalCostInPence() - basket.GetTotalDiscountsInPence());
        }

        [Test]
        public void GetTotalCostInPence_DiscountCodeBDDWith7Items_DiscountAppliedOnce()
        {
            var basket = new Basket();
            var product1 = Product.Create("f1", "Apple", 50, out _);
            basket.AddProduct(product1);
            basket.AddProduct(product1);
            var product2 = Product.Create("d1", "Cola", 100, out _);
            basket.AddProduct(product2);
            basket.AddProduct(product2);
            var product3 = Product.Create("s1", "Cheese sandwich", 150, out _);
            basket.AddProduct(product3);
            basket.AddProduct(product3);
            basket.AddProduct(product3);

            basket.AddDiscount("BDD");

            Assert.AreEqual(2, basket.GetQuantity(product1));
            Assert.AreEqual(2, basket.GetQuantity(product2));
            Assert.AreEqual(3, basket.GetQuantity(product3));
            var expected = (product1.GetPriceInPence() * 2 + product2.GetPriceInPence() * 2 + product3.GetPriceInPence() * 3) * 0.9;
            Assert.AreEqual(expected, basket.GetTotalCostInPence() - basket.GetTotalDiscountsInPence());
        }
        [Test]
        public void GenerateReceipt_DiscountCodeAPPLE241AndBDD_AppliesBothDiscountsToReceipt()
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

            Assert.AreEqual(4, basket.GetQuantity(product1));
            Assert.AreEqual(1, basket.GetQuantity(product2));

            const string expectedReceipt = "=== Items ===\r\n4x Apple @ 50p = £2.00\r\n1x Pear @ 40p = £0.40\r\nSub-total = £2.40\r\n\r\n=== Discounts ===\r\n2x APPLE241 = -£1.00\r\n1x BDD = -£0.24\r\nSub-total = -£1.24\r\n\r\n=== Total ===\r\n£1.16p\r\n";

            Assert.AreEqual(expectedReceipt, basket.GenerateReceipt());
        }
    }
}
