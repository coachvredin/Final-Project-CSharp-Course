using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shop;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Tests
{
    [TestClass()]
    public class MainWindowTests
    {
        [TestMethod()]
        public void FirstAndLastProductsProperties()
        {
            List<Product> loadedProducts = MainWindow.ReadProductFile("Products.csv");

            // Properties of first product
            Assert.AreEqual("AT-ACT_Walker.jpg", loadedProducts[0].ImageFileName);
            Assert.AreEqual("AT-ACT Walker", loadedProducts[0].ProductTitle);
            Assert.AreEqual("All terrain armored cargo transport", loadedProducts[0].ProductText);
            Assert.AreEqual(226500000, loadedProducts[0].ProductPrice);

            // Properties of last product
            Assert.AreEqual("TIE-Fighter.jpg", loadedProducts[16].ImageFileName);
            Assert.AreEqual("TIE-Fighter", loadedProducts[16].ProductTitle);
            Assert.AreEqual("Standard Imperial starfighter - fast and agile!", loadedProducts[16].ProductText);
            Assert.AreEqual(132670000, loadedProducts[16].ProductPrice);
        }

        [TestMethod()]
        public void AllProductsLoaded()
        {
            List<Product> loadedProducts = MainWindow.ReadProductFile("Products.csv");
            Assert.AreEqual(17, loadedProducts.Count);
        }

        [TestMethod()]
        public void LoadSavedCart()
        {
            Dictionary<Product, int> loadedCart = MainWindow.LoadCart("Test_cart.csv");

            foreach (KeyValuePair<Product, int> pair in loadedCart)
            {
                Assert.AreEqual(pair.Value, loadedCart[pair.Key]);
            }

        }

        [TestMethod()]
        public void NoCart()
        {
            Dictionary<Product, int> loadedCart = MainWindow.LoadCart("");
            Assert.AreEqual(0, loadedCart.Count);
        }

        [TestMethod()]
        public void CouponCodeDictionaryProperties()
        {
            Dictionary<string, decimal> loadedCart = MainWindow.CreateCouponDictionary("couponcodes.csv");

            Assert.AreEqual((decimal)(0.3), loadedCart["blackfriday"]);
            Assert.AreEqual((decimal)(0.1), loadedCart["sales"]);
        }

        [TestMethod()]
        public void AllCouponsLoaded()
        {
            Dictionary<string, decimal> loadedCoupons = MainWindow.CreateCouponDictionary("couponcodes.csv");
            Assert.AreEqual(3, loadedCoupons.Count);
        }
    }
}