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
        public void TestCountChargeAccountValidation()
        {
            const int rows = 3;
            var cols = 6;
            long[][] accounts = new long[rows][]{new long[] { 5658845, 4520125, 7895122, 8777541, 8451277, 1302850 },
                                                 new long[] { 8080152, 4562555, 5552012, 5050552, 7825877, 1250255 },
                                                 new long[] { 1005231, 6545231, 3852085, 7576651, 7881200, 4581002 } };


            foreach (var innerArray in accounts)
            { int counts = innerArray.Count(); }

            var count = accounts.SelectMany(innerArray => innerArray).Count();

            Assert.AreEqual(18, count);
        }
    }
};

{
    [TestClass]
    public class TestMethod()
    {
        [TestMethod]
        public void TestChargeAccountValidation()
    {
            const int rows = 3;
            var cols = 6;
            long[][] accounts = new long[rows][]{new long[] { 5658845, 4520125, 7895122, 8777541, 8451277, 1302850 },
                                                 new long[] { 8080152, 4562555, 5552012, 5050552, 7825877, 1250255 },
                                                 new long[] { 1005231, 6545231, 3852085, 7576651, 7881200, 4581002 } };

            var account = 4562555;
            var found = false;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    if (account == accounts[row][col])
                    {
                        found = true;
                    }
                }
            }


            Assert.AreEqual(true, found);
    }
}
};