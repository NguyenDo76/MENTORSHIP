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
        public void TestPayrollProgram()
        {
            Console.WriteLine("Please enter the number of hours the employee has worked:");
            var hours = 50;
            while (InvalidHours(hours))
            {
                Console.WriteLine("Error: Invalid hours.");
                Console.WriteLine("Please enter the number of hours the employee has worked:");
                hours = 10;
            }
            Console.WriteLine("Please enter the employee's hourly pay rate:");
            var payRate = 50;
            while (InvalidPay(payRate))
            {
                Console.WriteLine("Error: Invalid payrate.");
                Console.WriteLine("Please enter the employee's hourly pay rate:");
                payRate = 18;
            }
            Console.WriteLine("The employee's gross pay is: ${0:F2}", payRate * hours);

        
        static bool InvalidHours(int hours)
        {
            bool check = false;
            if (hours < 0 || hours > 40)
            {
                check = true;
            }
            return check;
        }
        static bool InvalidPay(double payrate)
        {
            bool check = false;
            if (payrate < 7.50 || payrate > 18.25)
            {
                check = true;
            }
            return check;
        }

        Assert.AreEqual(180, payRate * hours);
        }
    }
};