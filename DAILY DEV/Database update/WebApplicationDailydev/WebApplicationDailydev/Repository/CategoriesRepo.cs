using WebApplicationDailydev.Model;
using System;
using System.Data.SqlClient;

namespace WebApplicationDailydev.Repository
{
    public class CategoriesRepository
    {
        string connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=DailyDev;Integrated Security=True;Encrypt=True;TrustServerCertificate=true;";

        public void Upsert(Categories categories)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "Select count (CategoryName) from Categories where CategoryName = @CategoryName";
                var command = new SqlCommand(sql, connection);
                

                command.Parameters.AddWithValue("@CategoryName", categories.CategoryName);
                var count = (int)command.ExecuteScalar();

                if (count > 0)
                {
                    Update(categories);
                }
                else
                {
                    Add(categories);
                }
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        public void Add(Categories categories)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "INSERT INTO Categories (CategoryName) VALUES (@CategoryName)";
                var command = new SqlCommand(sql, connection);
                
                
                command.Parameters.AddWithValue("@CategoryName", categories.CategoryName);

                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        public IEnumerable<Categories> GetAll()
        {
            var categories = new List<Categories>();
            using (var connection = new SqlConnection(connectionString))
            {

                connection.Open();

                string sql = "Select * from Categories";
                var command = new SqlCommand(sql, connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    categories.Add(new Categories
                    {
                        CategoryID = int.Parse(reader["CategoryID"] + ""),
                        CategoryName = reader["CategoryName"] + "",           
                    });
                }
                connection.Close();
            }
            return categories;

        }

        public Categories GetId(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {

                connection.Open();

                string sql = string.Format(@"Select * from Categories where CategoryID = {0}", id);
                var command = new SqlCommand(sql, connection);
                var reader = command.ExecuteReader();

                command.Parameters.AddWithValue("@CategoryID", id);

                if (reader.Read())
                {
                    return new Categories
                    {
                        CategoryID = int.Parse(reader["CategoryID"] + ""),
                        CategoryName = reader["CategoryName"] + "",
                    };
                }
                connection.Close();
            }
            return null;
        }

        public void Update(Categories categories)
        {

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = string.Format(@"Update Categories set CategoryName = @CategoryName where CategoryID = @CategoryID");
                var command = new SqlCommand(sql, connection);
               

                command.Parameters.AddWithValue("@CategoryID", categories.CategoryID);
                command.Parameters.AddWithValue("@CategoryName", categories.CategoryName);

                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = string.Format(@"Delete from Categories where CategoryID = @CategoryID");
                var command = new SqlCommand(sql, connection);
                

                command.Parameters.AddWithValue("@CategoryID", id);

                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}