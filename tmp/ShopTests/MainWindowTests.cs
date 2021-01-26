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
        public void FirstAndLastProductsPropertiesTest()
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
        public void NoCartTest()
        {
            Dictionary<Product, int> loadedCart = MainWindow.LoadCart(@"C:\Windows\Temp\cart.csv");
            Assert.AreEqual(0, loadedCart.Count);
        }

        [TestMethod()]
        public void AllCouponsTest()
        {
            Dictionary<string, decimal> loadedCart = MainWindow.CreateCouponDictionary(@"Couponcodes.csv");
            Assert.AreEqual(3, loadedCart.Count);
        }
    }
}