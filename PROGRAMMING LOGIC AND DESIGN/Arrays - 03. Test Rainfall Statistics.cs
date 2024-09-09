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
        public void TestRainfallStatistics()
        {
            var length = 12;
            var monthlyRainfall = new double[length];

            var total = 0.0;
            var avg = 0.0;

            for (int i = 0; i < length; i++)
            {
                var month = i+1;
                monthlyRainfall[i] = month;
                total += month;
                avg = total / month;

            }
            var highest = monthlyRainfall[0];
            var lowest = monthlyRainfall[0];
            for (int i = 1; i < length; i++)
            {
                if (highest < monthlyRainfall[i])
                {
                    highest = monthlyRainfall[i];
                }

                if (lowest > monthlyRainfall[i])
                {
                    lowest = monthlyRainfall[i];
                }

            }
            Assert.AreEqual(78, total);
            Assert.AreEqual(6.5, avg);
            Assert.AreEqual(12, highest);
            Assert.AreEqual(1, lowest);
        }
    }
};