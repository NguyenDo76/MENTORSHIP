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
        public void TestAverageAndGrade()
        {
            Console.WriteLine("Please enter five grades on seperate lines:");
            var firstGrade = 70;
            var secondGrade = 80;
            var thirdGrade = 90;
            var fourthGrade = 100;
            var fifthGrade = 50;
            var average = calcAverage(firstGrade, secondGrade, thirdGrade,fourthGrade, fifthGrade);
            var gradeLetter = determineGrade(thirdGrade);

            Console.WriteLine("The average is: {0}", calcAverage(firstGrade, secondGrade, thirdGrade,
                fourthGrade, fifthGrade));

        
        static int calcAverage(int first, int second, int third, int fourth, int fifth)
        {
            var average = (first + second + third + fourth + fifth) / 5;
            return average;
        }
        static string determineGrade(int grade)
        {
            var gradeLetter = "";
            if (grade >= 90 && grade <= 100)
            {
                gradeLetter = "A";
            }
            else if (grade < 90 && grade >= 80)
            {
                gradeLetter = "B";
            }
            else if (grade < 80 && grade >= 70)
            {
                gradeLetter = "C";
            }
            else if (grade < 70 && grade >= 60)
            {
                gradeLetter = "D";
            }
            else
            {
                gradeLetter = "F";
            }
            return gradeLetter;
        }

        Assert.AreEqual(78, average);
        Assert.AreEqual("A", gradeLetter);

        }
    }
};