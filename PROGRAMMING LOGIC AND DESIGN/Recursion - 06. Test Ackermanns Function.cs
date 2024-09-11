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
        public void TestAckermannsFunction()
        {
            Console.WriteLine("Please enter m:");
            var m = 2;
            Console.WriteLine("Please enter n:");
            var n = 3;

            Console.WriteLine("The result is: {0}", Ackermann(m, n));
            var result = (Ackermann(m, n));

        static int Ackermann(int m, int n)
        {
            if (m == 0)
            {
                return n + 1;
            }
            if (n == 0)
            {
                return Ackermann(m - 1, 1);
            }
            return Ackermann(m - 1, Ackermann(m, n - 1));
        }

        Assert.AreEqual(9, result);


        }
    }
};