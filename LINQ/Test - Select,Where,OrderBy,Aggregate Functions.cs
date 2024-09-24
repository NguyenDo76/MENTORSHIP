using System.Data.SqlTypes;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static unittest.TestMethod;


namespace unittest
{
    [TestClass]
    public class TestMethod()
    {
        //Using Select : The student whose age is between 12 and 20 => return Boolean
        [TestMethod]
        public void TestLinQSelect()
        {
            var students = CreateStudents();
            var result_select = students.Select(std => std.Age <= 20 && std.Age >= 12).ToList();

            Assert.IsTrue(result_select[0]);
            Assert.IsFalse(result_select[1]);
        }

        //Using Where, contain: The name of students contain 'o' 
        [TestMethod]
        public void TestLinQWhere()
        {
            var students = CreateStudents();
            var result_where_contain = students.Where(std => std.StudentName.Contains("o")).ToList();

            Assert.AreEqual("John", result_where_contain[0].StudentName);
        }

        //Using where, count
        [TestMethod]
        public void TestLinQWhereCount()
        {
            var students = CreateStudents();
            var resutl_where_count = students.Where(std => std.Age <= 20 && std.Age >= 12).Count();

            Assert.AreEqual (5, resutl_where_count);
        }

        //Using First
        [TestMethod]
        public void TestLinQFirst()
        {
            var students = CreateStudents();
            var result_first = students.First(std => std.Age > 20);

            Assert.AreEqual("Steve", result_first.StudentName);
        }

        //Using where,select,first
        [TestMethod]
        public void TestLinQWhere_select_first()
        {
            var students = CreateStudents();
            var TestLinQWhere_select_first = students.Where(std => std.Age > 20).Select(std => std.Age > 20).First();

            Assert.IsTrue(TestLinQWhere_select_first);
        }

        //Using Last
        [TestMethod]
        public void TestLinQLast()
        {
            var students = CreateStudents();
            var result_last = students.Last(std => std.Age > 20);

            Assert.AreEqual("Ron", result_last.StudentName);
        }

        //Using where,select,last
        [TestMethod]
        public void TestLinQWhere_select_last()
        {
            var students = CreateStudents();
            var TestLinQWhere_select_last = students.Where(std => std.Age > 20).Select(std => std.Age > 20).Last();

            Assert.IsTrue(TestLinQWhere_select_last);
        }

        //Using Orderby DESC, ASC
        [TestMethod]
        public void TestLinQOrderBy()
        {
            var students = CreateStudents();
            var result_DESC_Age = students.OrderByDescending(std => std.Age).ToList();
            var result_ASC_Age = students.OrderBy(std => std.Age).ToList();
            var result_DESC_Name = students.OrderByDescending(std => std.StudentName).ToList();
            var result_ASC_Name = students.OrderBy(std => std.StudentName).ToList();

            Assert.AreEqual(31, result_DESC_Age[0].Age);
            Assert.AreEqual(17, result_ASC_Age[0].Age);
            Assert.AreEqual("Steve", result_DESC_Name[0].StudentName);
            Assert.AreEqual("Bill", result_ASC_Name[0].StudentName);
        }

        //Using distinct
        [TestMethod]
        public void TestLinQDistinct()
        {
            var students = CreateStudents();
            var result_distinct = students.Select(std => new { std.StudentID, std.StudentName, std.Age }).Distinct().ToList();

            Assert.AreEqual("Bill", result_distinct[2].StudentName);
        }

        //Using All,Any,ElementAt
        [TestMethod]
        public void TestLinQAllAnyElementAt()
        {
            var students = CreateStudents();
            bool result_all = students.All(std => std.Age > 20);
            bool result_any = students.Any(std => std.Age > 20);
            var result_ElementAt = students.ElementAt(1);

            Assert.IsFalse(result_all);
            Assert.IsTrue(result_any);
            Assert.AreEqual("Steve",result_ElementAt.StudentName);
        }

        //Aggregate Functions
        [TestMethod]
        public void TestLinQAggFunc()
        {
            var students = CreateStudents();
            var result_min_age = students.Min(std => std.Age);
            var result_max_age = students.Max(std => std.Age);
            var result_sum_age = students.Sum(std => std.Age);
            var result_avg_age = students.Average(std => std.Age);
            var result_count_std = students.Select(std => new { std.StudentID, std.StudentName, std.Age }).Distinct().Count();

            Assert.AreEqual(17,result_min_age);
            Assert.AreEqual(31, result_max_age);
            Assert.AreEqual(170, result_sum_age);
            Assert.AreEqual(21.25, result_avg_age);
            Assert.AreEqual(7, result_count_std);
        }
        private List<Student> CreateStudents()
        {
            var Students = new List<Student>();
            {
                Students.Add(new Student { StudentID = 1, StudentName = "John", Age = 18 });
                Students.Add(new Student { StudentID = 2, StudentName = "Steve", Age = 21 });
                Students.Add(new Student { StudentID = 3, StudentName = "Bill", Age = 25 });
                Students.Add(new Student { StudentID = 4, StudentName = "Ram", Age = 20 });
                Students.Add(new Student { StudentID = 5, StudentName = "Ron", Age = 31 });
                Students.Add(new Student { StudentID = 6, StudentName = "Chris", Age = 17 });
                Students.Add(new Student { StudentID = 7, StudentName = "Rob", Age = 19 });
                Students.Add(new Student { StudentID = 7, StudentName = "Rob", Age = 19 });
            };
            return Students;
        }

        public class Student
        {
            public int StudentID { get; set; }
            public string StudentName { get; set; }
            public int? Age { get; set; }

        }
    }
};
