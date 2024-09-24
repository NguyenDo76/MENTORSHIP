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

            var Teachers = new List<Teacher>();
            {
                Teachers.Add(new Teacher { TeacherID = 1, TeacherName = "Alice" });
                Teachers.Add(new Teacher { TeacherID = 2, TeacherName = "Sammy" });
            };

            //GroupBy
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine("GroupBy BaseAges");
            var result_groupby = Students.GroupBy(
                std => std.BaseAge,
                std => std.Age,
                (baseAge, ages) => new
                {
                    Key = baseAge,
                    Count = ages.Distinct().Count(),
                    Min = ages.Min(),
                    Max = ages.Max(),
                });

            foreach ( var result in result_groupby)
            {
                Console.WriteLine("Age group:" + result.Key);
                Console.WriteLine("Number of students in this age group: {0}", result.Count);
                Console.WriteLine("Minimum age:" + result.Min);
                Console.WriteLine("Maximum age:" + result.Max);
            }


            //Join: InnerJoin, LeftJoin
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine("InnerJoin");
            var result_inner_join = (from Student in Students
                                    join Teacher in Teachers on Student.TeacherID equals Teacher.TeacherID
                                    select new
                                    {
                                        StudentID = $"{Student.StudentID}",
                                        StudentName = $"{Student.StudentName}",
                                        TeacherID = $"{Student.TeacherID}",
                                        StudentAge = $"{Student.Age}",
                                        StudentBaseAge = $"{Student.BaseAge}",
                                        TeacherName = $"{Teacher.TeacherName}"
                                    }).Distinct();
            foreach (var result in result_inner_join)
            {
                Console.WriteLine($"{result.StudentID} - {result.StudentName} - {result.TeacherID} - {result.StudentAge} - {result.StudentBaseAge} - {result.TeacherName}");
            }

            Console.WriteLine("InnerJoinMethod");
            var result_inner_join_method = Students.Join(
                Teachers,
                Student => Student.TeacherID, Teacher => Teacher.TeacherID,
                (Student, Teacher) => new
                {
                    StudentID = $"{Student.StudentID}",
                    StudentName = $"{Student.StudentName}",
                    TeacherID = $"{Student.TeacherID}",
                    StudentAge = $"{Student.Age}",
                    StudentBaseAge = $"{Student.BaseAge}",
                    TeacherName = $"{Teacher.TeacherName}"
                }).Distinct();
            foreach (var result in result_inner_join_method)
            {
                Console.WriteLine($"{result.StudentID} - {result.StudentName} - {result.TeacherID} - {result.StudentAge} - {result.StudentBaseAge} - {result.TeacherName}");
            }

            Console.WriteLine("LeftJoin");
            var result_left_join = (from Student in Students
                                     join Teacher in Teachers on Student.TeacherID equals Teacher.TeacherID into gj
                                     from subgroup in gj.DefaultIfEmpty()
                                     select new
                                     {
                                         StudentID = $"{Student.StudentID}",
                                         StudentName = $"{Student.StudentName}",
                                         TeacherID = $"{Student.TeacherID}",
                                         StudentAge = $"{Student.Age}",
                                         StudentBaseAge = $"{Student.BaseAge}",
                                         TeacherName = subgroup?.TeacherName ?? string.Empty
                                     }).Distinct();
            foreach (var result in result_left_join)
            {
                Console.WriteLine($"{result.StudentID} - {result.StudentName} - {result.TeacherID} - {result.StudentAge} - {result.StudentBaseAge} - {result.TeacherName}");
            }

            Console.WriteLine("LeftJoinMethod");
            var result_left_join_method = Students.GroupJoin(
                Teachers,
                Student => Student.TeacherID, Teacher => Teacher.TeacherID,
                (Student, TeacherList) => new
                {
                    StudentID = $"{Student.StudentID}",
                    StudentName = $"{Student.StudentName}",
                    TeacherID = $"{Student.TeacherID}",
                    StudentAge = $"{Student.Age}",
                    StudentBaseAge = $"{Student.BaseAge}",
                    subgroup = TeacherList.AsQueryable()
                }).SelectMany (joinedSet => joinedSet.subgroup.DefaultIfEmpty(),
                (Student,Teacher) => new
                {
                    StudentID = $"{Student.StudentID}",
                    StudentName = $"{Student.StudentName}",
                    TeacherID = $"{Student.TeacherID}",
                    StudentAge = $"{Student.Age}",
                    StudentBaseAge = $"{Student.BaseAge}",
                    TeacherName = $"{Teacher.TeacherName}"
                }).Distinct();
            foreach (var result in result_left_join_method)
            {
                Console.WriteLine($"{result.StudentID} - {result.StudentName} - {result.TeacherID} - {result.StudentAge} - {result.StudentBaseAge} - {result.TeacherName}");
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

    public class Teacher
    {
        public int TeacherID { get; set; }
        public string TeacherName { get; set; }
    }

}
