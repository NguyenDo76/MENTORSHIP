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
        public void TestKineticEngergy()
        {
            Console.WriteLine("Enter the objects's mass and velocity on seperate lines, velocity first:");
            var velocity = 7;
            var mass = 8;
            var kineticE = KineticEnergy (velocity, mass);

            Console.WriteLine("The object's kinetic engergy is: {0}", KineticEnergy(velocity, mass));
        
        static string KineticEnergy(int velocity, int mass)
        {
            var kineticE = 0.5 * mass * (velocity * velocity);
            return Convert.ToString(kineticE);
        }

        Assert.AreEqual("196", kineticE);
        }
    }
};