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
        public void TestDriversLicenseExam()
        {
            var limit = 15;
            var length = 20;
            var correctAnswers = new string[]   { "B", "D", "A", "A", "C", "A", "B", "A", "C", "D", "B", "C", "D", "A", "D", "C", "C", "B", "D", "A" };
            var answersToTheTest = new string[] { "C", "A", "B", "C", "A", "b", "B", "A", "C", "D", "B", "C", "D", "A", "D", "C", "C", "B", "D", "A" }; ;
            var countPositive = 0;
            var countNegative = 0;


            for (int i = 0; i < length; i++)
            {
                if (answersToTheTest[i].ToLower() == correctAnswers[i].ToLower())
                {
                    countPositive++;
                }
                else
                {
                    countNegative++;
                }
            }

            if (countPositive >= limit)
            {
                Console.WriteLine("Congrats! You have passed the test!");
                Console.WriteLine("{0} anwsers are correct",countPositive);
            }
            else
            {
                Console.WriteLine("I'm sorry, you have failed.");
                Console.WriteLine("{0} anwsers are not correct", countNegative);
            }
            
            for (int i = 0; i < length; i++)
            {
                if (answersToTheTest[i].ToLower() != correctAnswers[i].ToLower() && i != length - 1)
                {
                    Console.Write("{0}, ", i + 1);
                }
                else if (answersToTheTest[i].ToLower() != correctAnswers[i].ToLower() && i == length - 1)
                {
                    Console.Write("{0} ", i + 1);
                }
            }


            Assert.AreEqual(14, countPositive);
        }
    }
};