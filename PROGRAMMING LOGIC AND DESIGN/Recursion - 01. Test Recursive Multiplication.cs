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
        public void TestRecursiveMultiplication()
        {
            var x = 10;
            var y = 2;
            Console.WriteLine(multiply(x, y));

            var sum = x + multiply(x, y - 1);


        static int multiply(int x, int y)
        {

            if (y == 1)
            {
                return x;
            }
            var sum = x + multiply(x, y - 1);
            return sum;
        }

        Assert.AreEqual(20, sum);


        }
    }
};