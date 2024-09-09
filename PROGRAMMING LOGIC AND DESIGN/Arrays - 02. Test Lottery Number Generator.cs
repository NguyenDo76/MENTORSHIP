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
        public void TestLotteryNumberGenerator()
        {
        var length = 7;
        var lotteryNum = new int[length];
        Random rnd = new Random();

        for (int i = 0; i < length; i++)
        {
            lotteryNum[i] = rnd.Next(0, 10);
        }
        Assert.AreEqual(7, length);
        }
    }
};