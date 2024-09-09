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
    public void TestPayroll()
    {
            var length = 7;
            var empID = new int[] { 56588, 45201, 78951, 87775, 84512, 13028, 75804 };
            var hours = new int[length];
            var payRate = new double[length];
            var wages = new double[length];

            for (int i = 0; i < length; i++)
            {
                Console.WriteLine("Employee {0}", empID[i]);
                Console.WriteLine("Please enter the hours worked for the employee:");
                var employeeID = 56588;
                empID[i] = employeeID;
                var hour = 8;
                hours[i] = hour;
                Console.WriteLine("Please enter the pay rate for the employee:");
                var pay = 200;
                payRate[i] = pay;
                wages[i] = hour * pay;
            }

            Console.WriteLine();

            for (int i = 0; i < length; i++)
            {
                Console.WriteLine("Employee {0} total wages are: {1:F2}$", empID[i], wages[i]);
            }

            Assert.AreEqual(1600, wages[0]);
    }
}
};