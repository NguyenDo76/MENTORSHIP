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
        public void TestRecursivePowerMethod()
        {
            Console.WriteLine("Please enter the number to be raised:");
            var number = 2;
            Console.WriteLine("Please enter the power:");
            var power = 3;
            Console.WriteLine("{0} raised to the power of {1} is {2}", number, power, RaisePower(number, power));
            var result = RaisePower(number, power);
        static int RaisePower(int number, int power)
        {
            if (power == 1) // n^1 = n
            {
                return number;
            }

            var product = number * RaisePower(number, power - 1);
            return product;
        }

        Assert.AreEqual(8, result);


        }
    }
};