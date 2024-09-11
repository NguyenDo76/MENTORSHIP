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
        public void TestSumOfNumbers()
        {
            var number = 20;

            Console.WriteLine("The sum of all the numbers up to that number is: {0}", NumberSum(number));
            var result = NumberSum(number);
        
        static int NumberSum(int number)
        {
            if (number == 0)
            {
                return number;
            }

            var sum = number + NumberSum(number - 1);
            return sum;

        }

        Assert.AreEqual(210, result);


        }
    }
};