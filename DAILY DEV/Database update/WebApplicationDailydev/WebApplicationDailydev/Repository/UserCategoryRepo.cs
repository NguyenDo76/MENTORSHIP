using WebApplicationDailydev.Model;
using System;
using System.Data.SqlClient;

namespace WebApplicationDailydev.Repository
{
    public class UserCategoryRepository
    {
        string connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=DailyDev;Integrated Security=True;Encrypt=True;TrustServerCertificate=true;";

    
        public void Add(UserCategory userCategory)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "INSERT INTO UserCategory (UserID, CategoryFlavorID, CreatedDate) VALUES (@UserID, @CategoryFlavorID, @CreatedDate)";
                var command = new SqlCommand(sql, connection);


                command.Parameters.AddWithValue("@UserID", userCategory.UserID);
                command.Parameters.AddWithValue("@CategoryFlavorID", userCategory.CategoryFlavorID);
                command.Parameters.AddWithValue("@CreatedDate", userCategory.CreatedDate);


                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        public IEnumerable<UserCategory> GetAll()
        {
            var usercategory = new List<UserCategory>();
            using (var connection = new SqlConnection(connectionString))
            {

                connection.Open();

                string sql = "Select * from UserCategory";
                var command = new SqlCommand(sql, connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    usercategory.Add(new UserCategory
                    {
                        UserCategoryID = int.Parse(reader["UserCategoryID"] + ""),
                        UserID = int.Parse(reader["UserID"] + ""),
                        CategoryFlavorID = int.Parse(reader["CategoryFlavorID"] + ""),
                        CreatedDate = (DateTime) reader["CreatedDate"],
                    });
                }
                connection.Close();
            }
            return usercategory;

        }

        public UserCategory GetId(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {

                connection.Open();

                string sql = string.Format(@"Select * from UserCategory where UserCategoryID = {0}", id);
                var command = new SqlCommand(sql, connection);
                var reader = command.ExecuteReader();

                command.Parameters.AddWithValue("@UserCategoryID", id);

                if (reader.Read())
                {
                    return new UserCategory
                    {
                        UserCategoryID = int.Parse(reader["UserCategoryID"] + ""),
                        UserID = int.Parse(reader["UserID"] + ""),
                        CategoryFlavorID = int.Parse(reader["CategoryFlavorID"] + ""),
                        CreatedDate = (DateTime)reader["CreatedDate"],
                    };
                }
                connection.Close();
            }
            return null;
        }

        public void Update(UserCategory userCategory)
        {

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = string.Format(@"Update UserCategory set UserID = @UserID,
                                                                     CategoryFlavorID = @CategoryFlavorID,
                                                                     CreatedDate = @CreatedDate,                                                                    
                                                               where UserCategoryID = @UserCategoryID");
                var command = new SqlCommand(sql, connection);


                command.Parameters.AddWithValue("@UserCategoryID", userCategory.UserCategoryID);
                command.Parameters.AddWithValue("@UserID", userCategory.UserID);
                command.Parameters.AddWithValue("@CategoryFlavorID", userCategory.CategoryFlavorID);
                command.Parameters.AddWithValue("@CreatedDate", userCategory.CreatedDate);

                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = string.Format(@"Delete from UserCategory where UserCategoryID = @UserCategoryID");
                var command = new SqlCommand(sql, connection);


                command.Parameters.AddWithValue("@UserCategoryID", id);

                command.ExecuteNonQuery();
                connection.Close();
            }
        }        
    }
}