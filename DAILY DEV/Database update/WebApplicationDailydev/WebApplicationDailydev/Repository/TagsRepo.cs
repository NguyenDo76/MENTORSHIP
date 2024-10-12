using WebApplicationDailydev.Model;
using System;
using System.Data.SqlClient;

namespace WebApplicationDailydev.Repository
{
    public class TagsRepository
    {
        string connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=DailyDev;Integrated Security=True;Encrypt=True;TrustServerCertificate=true;";

        public void Upsert(Tags tags)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "Select count (TagName) from Tags where TagName = @TagName";
                var command = new SqlCommand(sql, connection);


                command.Parameters.AddWithValue("@TagName", tags.TagName);
                var count = (int)command.ExecuteScalar();

                if (count > 0)
                {
                    Update(tags);
                }
                else
                {
                    Add(tags);
                }
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        public void Add(Tags tags)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "INSERT INTO Tags (TagName) VALUES (@TagName)";
                var command = new SqlCommand(sql, connection);


                command.Parameters.AddWithValue("@TagName", tags.TagName);

                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        public IEnumerable<Tags> GetAll()
        {
            var tags = new List<Tags>();
            using (var connection = new SqlConnection(connectionString))
            {

                connection.Open();

                string sql = "Select * from Tags";
                var command = new SqlCommand(sql, connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    tags.Add(new Tags
                    {
                        TagID = int.Parse(reader["TagID"] + ""),
                        TagName = reader["TagName"] + "",
                    });
                }
                connection.Close();
            }
            return tags;

        }

        public Tags GetId(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {

                connection.Open();

                string sql = string.Format(@"Select * from Tags where TagID = {0}", id);
                var command = new SqlCommand(sql, connection);
                var reader = command.ExecuteReader();

                command.Parameters.AddWithValue("@TagID", id);

                if (reader.Read())
                {
                    return new Tags
                    {
                        TagID = int.Parse(reader["TagID"] + ""),
                        TagName = reader["TagName"] + "",
                    };
                }
                connection.Close();
            }
            return null;
        }

        public void Update(Tags tags)
        {

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = string.Format(@"Update Tags set TagName = @TagName where TagID = @TagID");
                var command = new SqlCommand(sql, connection);


                command.Parameters.AddWithValue("@TagID", tags.TagID);
                command.Parameters.AddWithValue("@TagName", tags.TagName);

                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = string.Format(@"Delete from Tags where TagID = @TagID");
                var command = new SqlCommand(sql, connection);


                command.Parameters.AddWithValue("@TagID", id);

                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}