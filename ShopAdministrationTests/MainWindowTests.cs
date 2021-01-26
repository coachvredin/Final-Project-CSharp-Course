using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShopAdministration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace ShopAdministration.Tests
{
    [TestClass()]
    public class MainWindowTests
    {
        //[TestMethod()]
        //public void CouponRulesEmptyDiscountBox()
        //{
        //    TextBox couponCodeBox = new TextBox
        //    {
        //        Text = "New Coupon"
        //    };

        //    TextBox couponDiscountBox = new TextBox
        //    {
        //        Text = ""
        //    };

        //    bool check = MainWindow.CheckCouponRules();

        //    Assert.Fail(false.ToString(), check);
        //}

        [TestMethod()]
        public void CouponRulesEmptyDiscountBox()
        {
            TextBox couponCodeBox = new TextBox
            {
                Text = "New Coupon"
            };

            TextBox couponDiscountBox = new TextBox
            {
                Text = ""
            };

            bool check = MainWindow.CheckCouponRules();

            Assert.AreEqual(false, check);
        }

    }
}