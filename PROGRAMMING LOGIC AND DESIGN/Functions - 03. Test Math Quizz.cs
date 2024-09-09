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
        public void TestMathQuizz()
        {
            
            var firstNum = 70;
            var secondNum = 80;
            var sumString = TestAnswer (firstNum, secondNum);

            Console.WriteLine("Guess what's the result of the equation:");
            Console.WriteLine(firstNum);
            Console.WriteLine("+ {0}", secondNum);
            Console.WriteLine(TestAnswer(firstNum, secondNum));

        static string TestAnswer(int first, int second)
        {
            var answer = 150;
            var sum = first + second;
            var sumString = "Wrong! The correct answer is " + Convert.ToString(first + second);
            if (sum == answer)
            {
                return "Congratulations, your answer is correct!";
            }
            else
            {
                return sumString;
            }
        }

        Assert.AreEqual("Congratulations, your answer is correct!", sumString);
        }
    }
};