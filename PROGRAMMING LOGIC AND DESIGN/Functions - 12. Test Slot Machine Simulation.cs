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
        public void TestSlotMachineSimulation()
        {
        
               
              
                var sum = 0;
                var total = 0;

                
                
                    Console.WriteLine("Enter the amount of money you want to insert:");
                    var money = 2000;
                    var temp = "";
                    var secondTemp = "";
                    var countSymbols = 0;
                    total += money;

                    for (int i = 1; i <= 3; i++)
                    {
                        var computerChoice = 6;
                        var newChoice = NewSymbol(computerChoice);
                        Console.WriteLine(newChoice);
                        if (temp == newChoice || newChoice == secondTemp)
                        {
                            countSymbols++;

                        }
                        if (i == 1)
                        {
                            temp = newChoice;
                        }
                        if (i == 2)
                        {
                            secondTemp = newChoice;
                        }


                    }
                    if (countSymbols == 1)
                    {
                        sum += money * 2;
                    }
                    else if (countSymbols == 2)
                    {
                        sum += money * 3;
                    }
                   
             

                Console.WriteLine("The total amount you've entered is: {0}", total);
                Console.WriteLine("The amount you've won is: {0}", sum);
            

            static string NewSymbol(int choice)
            {
                var symbol = "";
                if (choice == 1)
                {
                    symbol = "cherrie";
                }
                else if (choice == 2)
                {
                    symbol = "orange";
                }
                else if (choice == 3)
                {
                    symbol = "plum";
                }
                else if (choice == 4)
                {
                    symbol = "bell";
                }
                else if (choice == 5)
                {
                    symbol = "melon";
                }
                else if (choice == 6)
                {
                    symbol = "bar";
                }
                return symbol;
            }


        Assert.AreEqual(2000, total);
        Assert.AreEqual(6000, sum);


        }
    }
};