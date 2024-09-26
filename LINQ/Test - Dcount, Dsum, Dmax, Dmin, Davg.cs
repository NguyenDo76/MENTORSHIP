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
        //Dcount, Dsum, Dmax, Dmin, DAvg
        [TestMethod]
        public void TestLinQD()
        {
            var students = CreateStudents();

            var groupedStudents = students
            .GroupBy(s => s.BaseAge)
            .Select(g => new { BaseAge = g.Key, StudentIDs = g.Select(s => s.StudentID).Distinct(), StudentAge = g.Select(s => s.Age) }).ToList();

            Assert.AreEqual(1, groupedStudents[2].StudentIDs.Count());
            Assert.AreEqual(19, groupedStudents[0].StudentAge.Max());
            Assert.AreEqual(17, groupedStudents[0].StudentAge.Min());
            Assert.AreEqual(18.25, groupedStudents[0].StudentAge.Average());
            Assert.AreEqual(31, groupedStudents[2].StudentAge.Sum());
        }

        private List<Student> CreateStudents()
        {
            var Students = new List<Student>();
            {
                Students.Add(new Student { StudentID = 1, StudentName = "John", TeacherID = 1, Age = 18, BaseAge = " under 20" });
                Students.Add(new Student { StudentID = 2, StudentName = "Steve", TeacherID = 1, Age = 21, BaseAge = "20 to 30" });
                Students.Add(new Student { StudentID = 3, StudentName = "Bill", TeacherID = 1, Age = 25, BaseAge = "20 to 30" });
                Students.Add(new Student { StudentID = 4, StudentName = "Ram", TeacherID = 2, Age = 20, BaseAge = "20 to 30" });
                Students.Add(new Student { StudentID = 5, StudentName = "Ron", TeacherID = 2, Age = 31, BaseAge = " over 30" });
                Students.Add(new Student { StudentID = 6, StudentName = "Chris", TeacherID = 1, Age = 17, BaseAge = " under 20" });
                Students.Add(new Student { StudentID = 7, StudentName = "Rob", TeacherID = 2, Age = 19, BaseAge = " under 20" });
                Students.Add(new Student { StudentID = 7, StudentName = "Rob", TeacherID = 2, Age = 19, BaseAge = " under 20" });
            };
            return Students;
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
};
