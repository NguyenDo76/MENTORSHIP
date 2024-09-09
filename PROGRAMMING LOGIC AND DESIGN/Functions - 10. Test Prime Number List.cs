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
        public void TestPrimeNumberList()
        {
            var number = 2;
            var prime = isPrime(number);

            {

                if (isPrime(number))
                {
                    Console.WriteLine("The number {0} is prime!", number);
                }
                else
                {
                    Console.WriteLine("The number {0} is not a prime!", number);
                }
            }

        static bool isPrime(int number)
        {
            bool prime = true;
            for (int i = 1; i <= number; i++)
            {
                if (i != 1 && i != number && number % i == 0)
                {
                    prime = false;
                }
            }
            return prime;

        }

        Assert.IsTrue(prime);


        }
    }
};