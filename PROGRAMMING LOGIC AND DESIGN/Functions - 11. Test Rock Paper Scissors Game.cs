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
        public void TestRockPaperScissorsGame()
        {

                
                var computerChoice = 2;
                var computer = "";
                if (computerChoice == 1)
                {
                    computer = "rock";
                }
                else if (computerChoice == 2)
                {
                    computer = "paper";
                }
                else if (computerChoice == 3)
                {
                    computer = "scissors";
                }

                Console.WriteLine("Make your choice:");
                var choice = "scissors";
                Console.WriteLine("The computer's choice is {0}!", computer);
                var result = compareChoices(computer, choice);
                Console.WriteLine(result);

 
        static string compareChoices(string computer, string choice)
        {
            var result = "";
            if (choice.ToLower() == computer)
            {
                result = "You'll have to play again!";
            }
            else if (choice.ToLower() == "rock" && computer == "scissors")
            {
                result = "You win!";
            }

            else if (choice.ToLower() == "scissors" && computer == "rock")
            {
                result = "You lose!";
            }
            else if (choice.ToLower() == "scissors" && computer == "paper")
            {
                result = "You lose!";
            }
            else if (choice.ToLower() == "paper" && computer == "scissors")
            {
                result = "You win!";
            }
            else if (choice.ToLower() == "paper" && computer == "rock")
            {
                result = "You win!";
            }
            else if (choice.ToLower() == "rock" && computer == "paper")
            {
                result = "You lose!";
            }

            return result;
        }


        Assert.AreEqual("You lose!", result);

        }
    }
};