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
        public void TestOddEvenCounter()
        {
            var oddCount = 0;
            var evenCount = 0;

            for (int i = 1; i <= 10; i++)
            {

                if (oddEven(i))
                {
                    evenCount++;
                }
                else
                {
                    oddCount++;
                }
            }
            Console.WriteLine("{0} number are even.", evenCount);
            Console.WriteLine("{0} number are odd.", oddCount);
        
        static bool oddEven(int i)
        {
            bool flag;
            if (i % 2 == 0)
            {
                flag = true;
            }
            else
            {
                flag = false;
            }
            return flag;

        }

        Assert.AreEqual(5, oddCount);
        Assert.AreEqual(5, evenCount);
        
        }
    }
};