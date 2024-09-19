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
            { Students.Add(new Student { StudentID = 1, StudentName = "John", Age = 18 });
                Students.Add(new Student { StudentID = 2, StudentName = "Steve", Age = 21 });
                Students.Add(new Student { StudentID = 3, StudentName = "Bill", Age = 25 });
                Students.Add(new Student { StudentID = 4, StudentName = "Ram", Age = 20 });
                Students.Add(new Student { StudentID = 5, StudentName = "Ron", Age = 31 });
                Students.Add(new Student { StudentID = 6, StudentName = "Chris", Age = 17 });
                Students.Add(new Student { StudentID = 7, StudentName = "Rob", Age = 19 });
                Students.Add(new Student { StudentID = 7, StudentName = "Rob", Age = 19 }); };

            //Print list
            foreach (var student in Students)
            {
                Console.WriteLine("StudentID: {0}, StudentName: {1}, Age: {2}", student.StudentID, student.StudentName, student.Age);
            }

            //Using Select : The student whose age is between 12 and 20 => return Boolean
            var result_select = Students.Select(std => std.Age <= 20 && std.Age >= 12).ToList();
            foreach (var student in result_select)
            {
                Console.WriteLine(student);
            }

            //Using Where, contain: The name of students contain 'o' 
            var result_where_contain = Students.Where(std => std.StudentName.Contains("o"));
            foreach (var Student in result_where_contain)
            {
                Console.WriteLine("StudentName: {0}, Age: {1}", Student.StudentName, Student.Age);
            }

            //Using where, count and print list
            var resutl1 = Students.Where(std => std.Age <= 20 && std.Age >= 12);
            ///var resutl1 = Students.Count(std => std.Age <= 20 && std.Age >= 12); (Using Count directly)
            Console.WriteLine("Question: Count number of students whose age is between 12 and 20 is: {0}", resutl1.Count());
            Console.WriteLine("List is:");
            foreach (var student in resutl1)
            {
                Console.WriteLine("StudentID: {0}, Name: {1}, Age: {2}", student.StudentID, student.StudentName, student.Age);
            }

            //Using First
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine("Question: Should return student Id have age is bigger 20");
            var result3 = Students.First(std => std.Age > 20);
            Console.WriteLine("Student ID of the first student whose age is bigger than 20: {0}", result3.StudentID);

            //Using where,select,first
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine("");
            var result4 = Students.Where(std => std.Age > 20).Select(std => std.Age > 20).First();
            Console.WriteLine(result4);

            //Using Orderby DESC, ASC
            Console.WriteLine("----------------------------------------------------");
            var result_DESC_Age = Students.OrderByDescending(std => std.Age);
            Console.WriteLine("Sort by Age descending: ");
            foreach (var student in result_DESC_Age)
            {
                Console.WriteLine("StudentID: {0}, StudentName: {1}, Age: {2}", student.StudentID, student.StudentName, student.Age);
            }
            var result_ASC_Age = Students.OrderBy(std => std.Age);
            Console.WriteLine("Sort by Age asccending: ");
            foreach (var student in result_ASC_Age)
            {
                Console.WriteLine("StudentID: {0}, StudentName: {1}, Age: {2}", student.StudentID, student.StudentName, student.Age);
            }
            var result_DESC_Name = Students.OrderByDescending(std => std.StudentName);
            Console.WriteLine("Sort by Name descending: ");
            foreach (var student in result_DESC_Name)
            {
                Console.WriteLine("StudentID: {0}, StudentName: {1}, Age: {2}", student.StudentID, student.StudentName, student.Age);
            }
            var result_ASC_Name = Students.OrderBy(std => std.StudentName);
            Console.WriteLine("Sort by Name asccending: ");
            foreach (var student in result_ASC_Name)
            {
                Console.WriteLine("StudentID: {0}, StudentName: {1}, Age: {2}", student.StudentID, student.StudentName, student.Age);
            }

            //Using distinct
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine("List of student not duplicate:");
            var result_distinct = Students.Select(std => new {std.StudentID,std.StudentName,std.Age}).Distinct();
            foreach (var student in result_distinct)
            {
                Console.WriteLine("StudentID: {0}, StudentName: {1}, Age: {2}", student.StudentID, student.StudentName,student.Age);
            }

            //Using All,Any,ElementAt
            Console.WriteLine("----------------------------------------------------");
            bool result_all = Students.All(std => std.Age > 20);
            Console.WriteLine("{0} student have age > 20 ",result_all? "All": "Not all");
            bool result_any = Students.Any (std => std.Age > 35);
            Console.WriteLine("{0} student over 35 ages ", result_any ? "Have" : "Haven't");
            var result_ElementAt = Students.ElementAt(1);
            Console.WriteLine("The StudentName at second position is: '{0}'",result_ElementAt.StudentName);

            //Aggregate Functions
            Console.WriteLine("----------------------------------------------------");
            var result_min_age = Students.Min(std => std.Age);
            Console.WriteLine("The student have minimum age is: {0}", result_min_age);
            var result_max_age = Students.Max(std => std.Age);
            Console.WriteLine("The student have maximum age is: {0}", result_max_age);
            var result_sum_age = Students.Sum(std => std.Age);
            Console.WriteLine("The total of age in student list is: {0}", result_sum_age);
            var result_avg_age = Students.Average(std => std.Age);
            Console.WriteLine("The average of age in student list is: {0}", result_avg_age);
            var result_count_std = Students.Select(std => new { std.StudentID, std.StudentName, std.Age }).Distinct().Count();
            Console.WriteLine("Count of student in student list is: {0}", result_count_std);
        }
    }
    public class Student
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public int? Age { get; set; }
        
    }
}
