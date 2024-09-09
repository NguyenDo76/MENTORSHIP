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
        public void TestDaysOfEachMonth()
    {
            var length = 12;
            var numberOfDays = new int[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            var months = new string[]{"January","February","March", "April", "May", "June", "July","August",
                 "September",  "October","November", "December"  };

            var countmonth = months.Count();

            for (int i = 0; i < length; i++)
            {
                Console.WriteLine("{0} has {1}.", months[i], numberOfDays[i]);
            }

            Assert.AreEqual(12, countmonth);
    }
}
};