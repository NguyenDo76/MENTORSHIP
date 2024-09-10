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
        public void TestTheaterSeatingRevenue()
        {
            Console.WriteLine("Please enter the number of tickets sold for section A:");
            var ticketsA = 301;
            while (InvalidSeatsA(ticketsA))
            {
                Console.WriteLine("Please enter a valid number for the tickets sold:");
                ticketsA = 200;
            }


            Console.WriteLine("Please enter the number of tickets sold for section B:");
            var ticketsB = 501;
            while (InvalidSeatsB(ticketsB))
            {
                Console.WriteLine("Please enter a valid number for the tickets sold:");
                ticketsB = 200;
            }


            Console.WriteLine("Please enter the number of tickets sold for section C:");
            var ticketsC = 201;
            while (InvalidSeatsC(ticketsC))
            {
                Console.WriteLine("Please enter a valid number for the tickets sold:");
                ticketsC = 100;
            }

            var ticketsSold = GetTicketsRevenue(ticketsA, ticketsB, ticketsC);
            Console.WriteLine("The total revenue for the theater is: ${0:F2})", ticketsSold);

        

        static bool InvalidSeatsA(int seatsA)
        {
            bool check = false;
            if (seatsA < 0 || seatsA > 300)
            {
                check = true;
            }
            return check;
        }
        static bool InvalidSeatsB(int seatsB)
        {
            bool check = false;
            if (seatsB < 0 || seatsB > 500)
            {
                check = true;
            }
            return check;
        }
        static bool InvalidSeatsC(int seatsC)
        {
            bool check = false;
            if (seatsC < 0 || seatsC > 200)
            {
                check = true;
            }
            return check;
        }
        static int GetTicketsRevenue(int seatsA, int seatsB, int seatsC)
        {
            var gross = (20 * seatsA) + (15 * seatsB) + (10 * seatsC);
            return gross;
        }
        Assert.AreEqual(8000, ticketsSold);
        }
    }
};