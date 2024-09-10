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
        public void TestFatGramCalculator()
        {
            Console.WriteLine("Please enter the number of fat grams in the item:");
            var fatGrams = -1;
            while (InvalidGrams(fatGrams))
            {
                Console.WriteLine("Please enter a valid number for the fat grams:");
                fatGrams = 50;
            }

            Console.WriteLine("Please enter the number of calories in the item:");
            var calories = -1;
            while (InvalidCalories(calories))
            {
                Console.WriteLine("Please enter a valid number for the calories:");
                calories = 100;
            }

            var percentage = CalcPercent(fatGrams, calories);
            if (percentage < 0.3)
            {
                Console.WriteLine("The percentage is: {0:F2}", percentage);
                Console.WriteLine("The food is low in fat.");
            }
            else
            {
                Console.WriteLine("The percentage is: {0:F2}", percentage);
            }


        
        static bool InvalidGrams(double fatGrams)
        {
            bool check = false;
            if (fatGrams < 0)
            {
                check = true;
            }
            return check;
        }
        static bool InvalidCalories(double calories)
        {
            bool check = false;
            if (calories < 0)
            {
                check = true;
            }
            return check;
        }

        static double CalcPercent(double grams, double calories)
        {
            var percent = (grams * 9) / calories;
            return percent;
        }
        Assert.AreEqual(4.5, percentage);


        }
    }
};