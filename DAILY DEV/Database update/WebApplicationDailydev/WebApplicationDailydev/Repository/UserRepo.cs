using WebApplicationDailydev.Model;
using System;
using System.Data.SqlClient;

namespace WebApplicationDailydev.Repository
{
    public class UserRepository
    {
        string connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=DailyDev;Integrated Security=True;Encrypt=True;TrustServerCertificate=true;";

        
        public void Add(User user)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "INSERT INTO User (UserName, UserPassword, Email, JoinedDate, LastedSignOut)" +
                    "                VALUES (@UserName, @UserPassword, @Email, @JoinedDate, @LastedSignOut)";
                var command = new SqlCommand(sql, connection);


                command.Parameters.AddWithValue("@UserName", user.UserName);
                command.Parameters.AddWithValue("@UserPassword", user.UserPassword);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@JoinedDate", user.JoinedDate);
                command.Parameters.AddWithValue("@LastedSignOut", user.LastedSignOut);


                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        public IEnumerable<User> GetAll()
        {
            var user = new List<User>();
            using (var connection = new SqlConnection(connectionString))
            {

                connection.Open();

                string sql = "Select * from User";
                var command = new SqlCommand(sql, connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    user.Add(new User
                    {
                        UserID = int.Parse(reader["UserID"] + ""),
                        UserName = reader["UserName"] + "",
                        UserPassword = reader["UserPassword"] + "",
                        Email = reader["Email"] + "",
                        JoinedDate = (DateTime) reader["JoinedDate"],
                        LastedSignOut = (DateTime)reader["LastedSignOut"],
                    });
                }
                connection.Close();
            }
            return user;

        }

        public User GetId(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {

                connection.Open();

                string sql = string.Format(@"Select * from User where UserID = {0}", id);
                var command = new SqlCommand(sql, connection);
                var reader = command.ExecuteReader();

                command.Parameters.AddWithValue("@UserID", id);

                if (reader.Read())
                {
                    return new User
                    {
                        UserID = int.Parse(reader["UserID"] + ""),
                        UserName = reader["UserName"] + "",
                        UserPassword = reader["UserPassword"] + "",
                        Email = reader["Email"] + "",
                        JoinedDate = (DateTime)reader["JoinedDate"],
                        LastedSignOut = (DateTime)reader["LastedSignOut"],
                    };
                }
                connection.Close();
            }
            return null;
        }

        public void Update(User user)
        {

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = string.Format(@"Update User set UserName = @UserName,
                                                             UserPassword = @UserPassword,
                                                             Email = @Email,
                                                             JoinedDate = @JoinedDate,
                                                             LastedSignOut = @LastedSignOut
                                                         where UserID = @UserID");
                var command = new SqlCommand(sql, connection);


                command.Parameters.AddWithValue("@UserID", user.UserID);
                command.Parameters.AddWithValue("@UserName", user.UserName);
                command.Parameters.AddWithValue("@UserPassword", user.UserPassword);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@JoinedDate", user.JoinedDate);
                command.Parameters.AddWithValue("@LastedSignOut", user.LastedSignOut);

                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = string.Format(@"Delete from User where UserID = @UserID");
                var command = new SqlCommand(sql, connection);


                command.Parameters.AddWithValue("@UserID", id);

                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        public void Upsert(User user)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "Select count (Email) from User where Email = @Email";
                var command = new SqlCommand(sql, connection);


                command.Parameters.AddWithValue("@UserName", user.UserName);
                command.Parameters.AddWithValue("@UserPassword", user.UserPassword);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@JoinedDate", user.JoinedDate);
                command.Parameters.AddWithValue("@LastedSignOut", user.LastedSignOut);

                var count = (int)command.ExecuteScalar();

                if (count > 0)
                {
                    Update(user);
                }
                else
                {
                    Add(user);
                }
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}