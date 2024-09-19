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
        public void TestLargestElement()
        {
            int [] arr = {5, 1, 2, 3, 2, 1};
            var length = arr.Length - 1;
            Console.WriteLine("The largest number in the array is: {0}", FindLargest(arr, length, arr[length]));
            var result = FindLargest(arr, length, arr[length]);
        
       static int FindLargest(int[] arr, int length, int max) // we need max to come from outside, so it doesn't change
        {
            if (length < 0)
            {
                return max;
            }
            if (arr[length] > max)
            {
                max = arr[length];
            }
            return FindLargest(arr, length - 1, max);

        }

        Assert.AreEqual(5, result);


        }
    }
};