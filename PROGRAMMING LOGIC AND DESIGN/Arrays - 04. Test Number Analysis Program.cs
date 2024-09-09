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
        public void TestNumberAnalysisProgram()
        {
            var length = 12;
            var numbers = new double[length];

            var total = 0.0;
            var avg = 0.0;

            for (int i = 0; i < length; i++)
            {
                var number = i + 1;
                numbers[i] = number;
                total += number;
                avg = total / length;

            }
            var highest = numbers[0];
            var lowest = numbers[0];
            for (int i = 1; i < length; i++)
            {
                if (highest < numbers[i])
                {
                    highest = numbers[i];
                }

                if (lowest > numbers[i])
                {
                    lowest = numbers[i];
                }

            }
        Assert.AreEqual(78, total);
        Assert.AreEqual(6.5, avg);
        Assert.AreEqual(12, highest);
        Assert.AreEqual(1, lowest);
        }
    }
};