using WebApplicationDailydev.Model;
using System;
using System.Data.SqlClient;

namespace WebApplicationDailydev.Repository
{
    public class SourceRepository
    {
        
        string connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=DailyDev;Integrated Security=True;Encrypt=True;TrustServerCertificate=true;";
            
        public void Upsert(Source source)
        {
            using (var connection = new SqlConnection(connectionString))
            {                
                connection.Open();
                string sql = string.Format(@"Select count (SourceName)
                                             from Source
                                             where SourceName = @SourceName");
                var command = new SqlCommand (sql,connection);                
                
               
                command.Parameters.AddWithValue("@SourceName", source.SourceName);
                
                var count = (int) command.ExecuteScalar();

                if (count > 0)
                {
                    Update(source);
                }
                else
                {
                    Add(source);
                }
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        public void Add(Source source)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "INSERT INTO Source (SourceName, URL, URLViewSource) VALUES (@SourceName, @URL, @URLViewSource)";
                var command = new SqlCommand(sql, connection);
                
               
                command.Parameters.AddWithValue("@SourceName", source.SourceName);
                command.Parameters.AddWithValue("@URL", source.URL);
                command.Parameters.AddWithValue("@URLViewSource", source.URLViewSource);

                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public IEnumerable<Source> GetAll()
        {
            var sources = new List<Source>();
            using (var connection = new SqlConnection(connectionString))
            {
                
                connection.Open();

                string sql = "Select * from Source";
                var command = new SqlCommand(sql, connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    sources.Add(new Source
                    {
                        SourceID = int.Parse(reader["SourceID"] + ""),
                        SourceName = reader["SourceName"] + "",
                        URL = reader["URL"] + "",
                        URLViewSource = reader["URLViewSource"] + "",
                    });
                }
                connection.Close();
            }
            return sources;

        }

        public Source GetId(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                
                connection.Open();
                
                string sql = string.Format(@"Select * from Source where SourceID = {0}",id);
                var command = new SqlCommand(sql,connection);
                var reader = command.ExecuteReader();

                command.Parameters.AddWithValue("@SourceID", id);
             
                    if (reader.Read())
                    {
                        return new Source
                        {
                            SourceID = int.Parse(reader["SourceID"] + ""),
                            SourceName = reader["SourceName"] + "",
                            URL = reader["URL"] + "",
                            URLViewSource = reader["@URLViewSource"] + "",
                        };
                    }
                connection.Close();
            }
            return null;
        }

        public void Update(Source source)
        {

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = string.Format(@"Update Source set SourceName = @SourceName,
                                                               URL = @URL,
                                                               URLViewSource = @URLViewSource
                                                         where SourceID = @SourceID");
                var command = new SqlCommand(sql, connection);
                
                
                command.Parameters.AddWithValue("@SourceID", source.SourceID);
                command.Parameters.AddWithValue("@SourceName", source.SourceName);
                command.Parameters.AddWithValue("@URL", source.URL);
                command.Parameters.AddWithValue("@URLViewSource", source.URLViewSource);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = string.Format(@"Delete from Source where SourceID = @SourceID");
                var command = new SqlCommand(sql, connection);
                

                command.Parameters.AddWithValue("@SourceID", id);

                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}