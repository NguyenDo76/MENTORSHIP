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
        public void TestFeetToInches()
        {
            Console.WriteLine("Please enter the nuber of feet:");
            var feet = 12;
            var inches = CalcInches(feet);
            Console.WriteLine("There are {0} inches in {1} feet", CalcInches(feet), feet);

        
        static int CalcInches(int feet)
        {
            var inches = feet * 12;
            return inches;
        }

        Assert.AreEqual(144, inches);
        }
    }
};