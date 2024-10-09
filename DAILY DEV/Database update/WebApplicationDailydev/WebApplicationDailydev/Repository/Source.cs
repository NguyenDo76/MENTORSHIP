using WebApplicationDailydev.Model;
using System;
using System.Data.SqlClient;

namespace WebApplicationDailydev.Repository
{
    public class SourceRepository
    {
        private readonly string _connectionString;

        public SourceRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void Upsert(Source source)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                // Kiểm tra xem mục nhập đã tồn tại hay chưa
                connection.Open();
                string sql = "Select count (SourceName) from Source where SourceName = @SourceName";
                var command = connection.CreateCommand();
                command.CommandText = sql;
                command.ExecuteNonQuery();
               
                command.Parameters.AddWithValue("@SourceName", source.SourceName);
                
                var count = (int) command.ExecuteScalar(); // ExecuteScalar() trả về giá trị đầu tiên của cột đầu tiên trong kết quả

                if (count > 0)
                {
                    Update(source);
                }
                else
                {
                    Add(source);
                }
            }
        }
        public void Add(Source source)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sql = "INSERT INTO Source (SourceName, URL) VALUES (@SourceName, @URL)";
                var command = connection.CreateCommand();
                command.CommandText = sql;
                command.ExecuteNonQuery();

                command.Parameters.AddWithValue("@SourceName", source.SourceName);
                command.Parameters.AddWithValue("@URL", source.URL);
                
                connection.Close();
            }
        }

        public IEnumerable<Source> GetAll()
        {
            var sources = new List<Source>();
            using (var connection = new SqlConnection(_connectionString))
            {
                
                connection.Open();

                string sql = "Select * from Source";
                var command = connection.CreateCommand();
                command.CommandText = sql;
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    sources.Add(new Source
                    {
                        SourceID = int.Parse(reader["SourceID"] + ""),
                        SourceName = reader["SourceName"] + "",
                        URL = reader["URL"] + "",
                    });
                }
                connection.Close();
            }
            return sources;

        }

        public Source GetId(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                
                connection.Open();
                
                string sql = string.Format(@"Select * from Source where SourceID = {0}",id);
                var command = new SqlCommand(sql,connection);
                var reader = command.ExecuteReader();

                command.Parameters.AddWithValue("@Id", id);
             
                    if (reader.Read())
                    {
                        return new Source
                        {
                            SourceID = int.Parse(reader["SourceID"] + ""),
                            SourceName = reader["SourceName"] + "",
                            URL = reader["URL"] + "",
                        };
                    }
                connection.Close();
            }
            return null;
        }

        public void Update(Source source)
        {

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = string.Format(@"Update Source set SourceName = @SourceName, URL = @URL where SourceID = @SourceID");
                var command = new SqlCommand(sql, connection);
                command.ExecuteNonQuery();
                
                command.Parameters.AddWithValue("@SourceID", source.SourceID);
                command.Parameters.AddWithValue("@SourceName", source.SourceName);
                command.Parameters.AddWithValue("@URL", source.URL);
                connection.Close();
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string sql = string.Format(@"Delete from Source where SourceID = @SourceID");
                var command = new SqlCommand(sql, connection);
                command.ExecuteNonQuery();

                command.Parameters.AddWithValue("@SourceID", id);
                connection.Close();
            }
        }
    }
}