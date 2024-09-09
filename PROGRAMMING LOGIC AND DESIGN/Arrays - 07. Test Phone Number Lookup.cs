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
    public void TestPhoneNumberLookup()
    {
            var length = 7;
            var names = new string[] { "Deepjack", "Christina", "Aleah", "Tem", "Veselina", "Hale", "Chelle" };
            var phoneNums = new long[]
            {
                3598811713, 3598820312, 3598876523, 3598865312, 3598892333, 3598854228, 3598856765
            };

            var name = "aleah";
            var found = false;
            for (int i = 0; i < length; i++)
            {
                if (names[i].ToLower().Contains(name.ToLower()))
                {
                    Console.WriteLine("The number of {0} is: +{1}", name, phoneNums[i]);
                    found = true;
                }
            }

            Assert.AreEqual(true, found);
        }
}
};