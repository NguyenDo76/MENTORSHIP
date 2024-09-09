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
        public void TestMaximumOfTwoValues()
        {

            Console.WriteLine("Please enter two number to be compared ot seperate lines:");
            var firstNum = 10;
            var secondNum = 20;
            var biggerNum = Max(firstNum, secondNum);
            Console.WriteLine("The bigger number is: {0}", Max(firstNum, secondNum));

        
        static int Max(int first, int second)
        {
            int biggerNum = 0;
            if (first > second)
            {
                biggerNum = first;

            }
            else if (second > first)
            {
                biggerNum = second;

            }
            return biggerNum;
        }

        Assert.AreEqual(20, biggerNum);
        }
    }
};