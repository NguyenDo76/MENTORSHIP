using System.Data.SqlTypes;
using System.Diagnostics;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace unittest
{
    [TestClass]
    public class TestMethod()
    {
        [TestMethod]
        public void TestTotalSales()
        {
            var length = 7;
            var sales = new int[length];
            var total = 0;

            for (int i = 0; i < length; i++)
            {
                var sale = i;
                sales[0] = sale;
                total += sale;
            }
            Assert.AreEqual(21, total);
        }
    }
};