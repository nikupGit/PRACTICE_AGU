using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommonRefactorTask.Service;


namespace Tests
{
    [TestClass]
    public class DiscountServiceTests
    {
        [TestMethod]
        public void CalculateDiscountPercentage_LessThan1000_Returns0()
        {
            decimal price = 500;
            var result = DiscountService.CalculateDiscountPercentage(price);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void CalculateDiscountPercentage_3000_Returns5()
        {
            decimal price = 3000;
            var result = DiscountService.CalculateDiscountPercentage(price);
            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void CalculateDiscountPercentage_6000_Returns10()
        {
            decimal price = 6000;
            var result = DiscountService.CalculateDiscountPercentage(price);
            Assert.AreEqual(10, result);
        }
    }
}