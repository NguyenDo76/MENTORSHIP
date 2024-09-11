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
        public void TestRecursiveArraySum()
        {
            int[] arr = {1, 2, 4, 6, 2};
            var length = arr.Length - 1;
            Console.WriteLine("The sum of the elements is: {0}", SumArr(arr, length));
            var result = SumArr(arr, length);


        static int SumArr(int[] arr, int length)
        {
            if (length == 0)
            {
                return arr[0];
            }

            var sum = arr[length] + SumArr(arr, length - 1);
            return sum;

        }

        Assert.AreEqual(15, result);


        }
    }
};