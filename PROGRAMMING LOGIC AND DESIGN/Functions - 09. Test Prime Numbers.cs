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
        public void TestPrimeNumbers()
        {
            var number = 10;
            var prime = isPrime(number);
            

            if (isPrime(number))
            {
                Console.WriteLine("The number is prime!");
            }
            else
            {
                Console.WriteLine("The number is not a prime!");
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

            Assert.IsFalse(prime);


        }
    }
};