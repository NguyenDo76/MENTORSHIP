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
        public void TestRectangleArea()
        {
            Console.WriteLine("Please enter the rectangle's width and length on seperate lines:");
            var width = 2;
            var length = 3;
            var area = CalcArea(width, length);
            Console.WriteLine("The area is: {0}", area);


        
        static int CalcArea(int width, int length)
        {
            var area = width * length;
            return area;
        }

        Assert.AreEqual(6,area);
        }
    }
};