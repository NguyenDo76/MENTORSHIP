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
        public void TestFallingDistance()
        {

            var i = 2;
            var time = i;
            var distance = FallingDistance(time);

            static string FallingDistance(int time)
        {
            double distance = 0.5 * 9.8 * (time * time);
            return Convert.ToString(distance);

        }

        Assert.AreEqual("19.6", distance);
        }
    }
};