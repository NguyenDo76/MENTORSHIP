using WebApplicationDailydev.Model;
using System;
using System.Data.SqlClient;

namespace WebApplicationDailydev.Repository
{
    public class UserTagsRepository
    {
        string connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=DailyDev;Integrated Security=True;Encrypt=True;TrustServerCertificate=true;";


        public void Add(UserTags userTags)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "INSERT INTO UserTags (UserID, TagFlavorID, CreatedDate) VALUES (@UserID, @TagFlavorID, @CreatedDate)";
                var command = new SqlCommand(sql, connection);


                command.Parameters.AddWithValue("@UserID", userTags.UserID);
                command.Parameters.AddWithValue("@TagFlavorID", userTags.TagFlavorID);
                command.Parameters.AddWithValue("@CreatedDate", userTags.CreatedDate);


                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        public IEnumerable<UserTags> GetAll()
        {
            var usertags = new List<UserTags>();
            using (var connection = new SqlConnection(connectionString))
            {

                connection.Open();

                string sql = "Select * from UserTags";
                var command = new SqlCommand(sql, connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    usertags.Add(new UserTags
                    {
                        UserTagID = int.Parse(reader["UserTagID"] + ""),
                        UserID = int.Parse(reader["UserID"] + ""),
                        TagFlavorID = int.Parse(reader["TagFlavorID"] + ""),
                        CreatedDate = (DateTime)reader["CreatedDate"],
                    });
                }
                connection.Close();
            }
            return usertags;

        }

        public UserTags GetId(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {

                connection.Open();

                string sql = string.Format(@"Select * from UserTags where UserTagID = {0}", id);
                var command = new SqlCommand(sql, connection);
                var reader = command.ExecuteReader();

                command.Parameters.AddWithValue("@UserTagID", id);

                if (reader.Read())
                {
                    return new UserTags
                    {
                        UserTagID = int.Parse(reader["UserTagID"] + ""),
                        UserID = int.Parse(reader["UserID"] + ""),
                        TagFlavorID = int.Parse(reader["TagFlavorID"] + ""),
                        CreatedDate = (DateTime)reader["CreatedDate"],
                    };
                }
                connection.Close();
            }
            return null;
        }

        public void Update(UserTags userTags)
        {

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = string.Format(@"Update UserTags set UserID = @UserID,
                                                                 TagFlavorID = @TagFlavorID,
                                                                 CreatedDate = @CreatedDate,                                                                    
                                                           where UserTagID = @UserTagID");
                var command = new SqlCommand(sql, connection);


                command.Parameters.AddWithValue("@UserTagID", userTags.UserTagID);
                command.Parameters.AddWithValue("@UserID", userTags.UserID);
                command.Parameters.AddWithValue("@TagFlavorID", userTags.TagFlavorID);
                command.Parameters.AddWithValue("@CreatedDate", userTags.CreatedDate);

                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = string.Format(@"Delete from UserTags where UserTagID = @UserTagID");
                var command = new SqlCommand(sql, connection);


                command.Parameters.AddWithValue("@UserTagID", id);

                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}