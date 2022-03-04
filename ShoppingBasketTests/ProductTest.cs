using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ShoppingBasket;

namespace ShoppingBasketTests
{
    [TestFixture]
    public class ProductTest
    {
        [TestCase("f1", "Apple", 50, "", 'f')]
        [TestCase("", "Apple", 50, Product.CodeValueError, 'f')]
        [TestCase(null, "Apple", 50, Product.CodeValueError, 'f')]
        [TestCase("f", "Apple", 50, Product.CodeValueError, 'f')]
        [TestCase("11", "Apple", 50, Product.CodeValueError, 'f')]
        [TestCase("1f", "Apple", 50, Product.CodeValueError, 'f')]
        [TestCase("f1", "", 50, Product.NameValueError, 'f')]
        [TestCase("f1", "Apple", -1, Product.PriceValueError, 'f')]
        public void Create_ValidParams_CreatesProduct(string code, string name, int priceInPence, string expectedErrorMsg, char expectedCategory)
        {
            var product = Product.Create(code, name, priceInPence, out var errorMsg);

            Assert.AreEqual(expectedErrorMsg, errorMsg);

            if(!string.IsNullOrEmpty(errorMsg)) return;

            Assert.IsNotNull(product);
            Assert.AreEqual(code, product.GetCode());
            Assert.AreEqual(name, product.GetName());
            Assert.AreEqual(priceInPence, product.GetPriceInPence());
            Assert.AreEqual(expectedCategory, product.Category);

        }
    }
}
