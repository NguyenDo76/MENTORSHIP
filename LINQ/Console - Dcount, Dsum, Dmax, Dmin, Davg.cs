using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Numerics;
using System.ComponentModel.DataAnnotations;


namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            var Students = new List<Student>();
            { 
                Students.Add(new Student { StudentID = 1, StudentName = "John", TeacherID = 1, Age = 18 , BaseAge = " under 20"});
                Students.Add(new Student { StudentID = 2, StudentName = "Steve", TeacherID = 1, Age = 21, BaseAge = "20 to 30" });
                Students.Add(new Student { StudentID = 3, StudentName = "Bill", TeacherID = 1, Age = 25, BaseAge = "20 to 30" });
                Students.Add(new Student { StudentID = 4, StudentName = "Ram", TeacherID = 2, Age = 20, BaseAge = "20 to 30" });
                Students.Add(new Student { StudentID = 5, StudentName = "Ron", TeacherID = 2, Age = 31, BaseAge = " over 30" });
                Students.Add(new Student { StudentID = 6, StudentName = "Chris", TeacherID = 1, Age = 17, BaseAge = " under 20" });
                Students.Add(new Student { StudentID = 7, StudentName = "Rob", TeacherID = 2, Age = 19, BaseAge = " under 20" });
                Students.Add(new Student { StudentID = 7, StudentName = "Rob", TeacherID = 2, Age = 19, BaseAge = " under 20" }); 
            };


            //Dcount, Dsum, Dmax, Dmin, DAvg
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine("Dcount");
            var groupedStudentsCount = Students
            .GroupBy(s => s.BaseAge)
            .Select(g => new { BaseAge = g.Key, StudentIDs = g.Select(s => s.StudentID).Distinct() });
            foreach (var group in groupedStudentsCount)
            {
                Console.WriteLine($"{group.BaseAge}: StudentID {{{string.Join(", ", group.StudentIDs)}}} ({group.StudentIDs.Count()} students)");
            }

            Console.WriteLine("Dmax");
            var groupedStudentsMax = Students
            .GroupBy(s => s.BaseAge)
            .Select(g => new { BaseAge = g.Key, StudentIDs = g.Select(s => s.StudentID).Distinct(), StudentAge = g.Select(s => s.Age) });
            foreach (var group in groupedStudentsMax)
            {
                Console.WriteLine($"{group.BaseAge}: StudentID {{{string.Join(", ", group.StudentIDs)}}} (The maximum age is: {group.StudentAge.Max()} year old)");
            }

            Console.WriteLine("Dmin");
            var groupedStudentsMin = Students
            .GroupBy(s => s.BaseAge)
            .Select(g => new { BaseAge = g.Key, StudentIDs = g.Select(s => s.StudentID).Distinct(), StudentAge = g.Select(s => s.Age) });
            foreach (var group in groupedStudentsMin)
            {
                Console.WriteLine($"{group.BaseAge}: StudentID {{{string.Join(", ", group.StudentIDs)}}} (The minimum age is: {group.StudentAge.Min()} year old)");
            }

            Console.WriteLine("Davg");
            var groupedStudentsAvg = Students
            .GroupBy(s => s.BaseAge)
            .Select(g => new { BaseAge = g.Key, StudentIDs = g.Select(s => s.StudentID).Distinct(), StudentAge = g.Select(s => s.Age) });
            foreach (var group in groupedStudentsAvg)
            {
                Console.WriteLine($"{group.BaseAge}: StudentID {{{string.Join(", ", group.StudentIDs)}}}: (The average age is: {group.StudentAge.Average()} year old)");
            }

            Console.WriteLine("Dsum");
            var groupedStudentsSum = Students
            .GroupBy(s => s.BaseAge)
            .Select(g => new { BaseAge = g.Key, StudentIDs = g.Select(s => s.StudentID).Distinct(), StudentAge = g.Select(s => s.Age) });
            foreach (var group in groupedStudentsSum)
            {
                Console.WriteLine($"{group.BaseAge}: StudentID {{{string.Join(", ", group.StudentIDs)}}}: (The sum age is: {group.StudentAge.Sum()} year old)");
            }

        }



    }

    public class Student
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public int TeacherID { get; set; }
        public int? Age { get; set; }
        public string BaseAge { get; set; }
    }

  

}
