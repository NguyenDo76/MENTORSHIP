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
    public void TestSpeedingViolationCalc()
    {
            Console.WriteLine("Please enter the speed limit:");
            var speedLimit = 100;
            while (InvalidSpeedLimit(speedLimit))
            {
                Console.WriteLine("Please enter a valid speed limit:");
                speedLimit = 50;
            }

            Console.WriteLine("Please enter the driver's speed:");
            var driverSpeed = 50;
            while (InvalidSpeed(speedLimit, driverSpeed))
            {
                Console.WriteLine("Please enter a valid speed:");
                driverSpeed = 150;
            }

            Console.WriteLine("The driver was doing {0} mph over the speed limit.", driverSpeed - speedLimit);



        
        static bool InvalidSpeedLimit(int speedLimit)
        {
            bool check = false;
            if (speedLimit < 20 || speedLimit > 70)
            {
                check = true;
            }
            return check;
        }
        static bool InvalidSpeed(int speedLimit, int speed)
        {
            bool check = false;
            if (speed <= speedLimit)
            {
                check = true;
            }
            return check;
        }
        Assert.AreEqual(100, driverSpeed - speedLimit);


    }
}
};