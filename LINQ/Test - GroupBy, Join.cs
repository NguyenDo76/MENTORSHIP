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
        //GroupBy
        [TestMethod]
        public void TestLinQGroupBy()
        {
            var students = CreateStudents();
            var result_groupby = students.GroupBy(
                std => std.BaseAge,
                std => std.Age,
                (baseAge, ages) => new
                {
                    Key = baseAge,
                    Count = ages.Distinct().Count(),
                    Min = ages.Min(),
                    Max = ages.Max(),
                }).ToList();

            Assert.AreEqual(3, result_groupby[0].Count);
        }

        //Join: InnerJoin, LeftJoin
        [TestMethod]
        public void TestLinQJoin()
        {
            var students = CreateStudents();
            var teachers = CreateTeachers();
            var result_inner_join = (from Student in students
                                     join Teacher in teachers on Student.TeacherID equals Teacher.TeacherID
                                     select new
                                     {
                                         StudentID = $"{Student.StudentID}",
                                         StudentName = $"{Student.StudentName}",
                                         TeacherID = $"{Student.TeacherID}",
                                         StudentAge = $"{Student.Age}",
                                         StudentBaseAge = $"{Student.BaseAge}",
                                         TeacherName = $"{Teacher.TeacherName}"
                                     }).Distinct().ToList();
            var result_inner_join_method = students.Join(
                teachers,
                Student => Student.TeacherID, Teacher => Teacher.TeacherID,
                (Student, Teacher) => new
                {
                    StudentID = $"{Student.StudentID}",
                    StudentName = $"{Student.StudentName}",
                    TeacherID = $"{Student.TeacherID}",
                    StudentAge = $"{Student.Age}",
                    StudentBaseAge = $"{Student.BaseAge}",
                    TeacherName = $"{Teacher.TeacherName}"
                }).Distinct().ToList();
            var result_left_join = (from Student in students
                                    join Teacher in teachers on Student.TeacherID equals Teacher.TeacherID into gj
                                    from subgroup in gj.DefaultIfEmpty()
                                    select new
                                    {
                                        StudentID = $"{Student.StudentID}",
                                        StudentName = $"{Student.StudentName}",
                                        TeacherID = $"{Student.TeacherID}",
                                        StudentAge = $"{Student.Age}",
                                        StudentBaseAge = $"{Student.BaseAge}",
                                        TeacherName = subgroup?.TeacherName ?? string.Empty
                                    }).Distinct().ToList();
            var result_left_join_method = students.GroupJoin(
                teachers,
                Student => Student.TeacherID, Teacher => Teacher.TeacherID,
                (Student, TeacherList) => new
                {
                    StudentID = $"{Student.StudentID}",
                    StudentName = $"{Student.StudentName}",
                    TeacherID = $"{Student.TeacherID}",
                    StudentAge = $"{Student.Age}",
                    StudentBaseAge = $"{Student.BaseAge}",
                    subgroup = TeacherList.AsQueryable()
                }).SelectMany(joinedSet => joinedSet.subgroup.DefaultIfEmpty(),
                (Student, Teacher) => new
                {
                    StudentID = $"{Student.StudentID}",
                    StudentName = $"{Student.StudentName}",
                    TeacherID = $"{Student.TeacherID}",
                    //StudentAge = $"{Student.Age}",
                    //StudentBaseAge = $"{Student.BaseAge}",
                    TeacherName = $"{Teacher.TeacherName}"
                }).Distinct().ToList();

            Assert.AreEqual("Alice", result_inner_join[0].TeacherName);
            Assert.AreEqual("Alice", result_inner_join_method[0].TeacherName);
            Assert.AreEqual("Alice", result_left_join[0].TeacherName);
            Assert.AreEqual("Alice", result_left_join_method[0].TeacherName);
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
        private List<Teacher> CreateTeachers()
        {
            var Teachers = new List<Teacher>();
            {
                Teachers.Add(new Teacher { TeacherID = 1, TeacherName = "Alice" });
                Teachers.Add(new Teacher { TeacherID = 2, TeacherName = "Sammy" });
            };
            return Teachers;
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
