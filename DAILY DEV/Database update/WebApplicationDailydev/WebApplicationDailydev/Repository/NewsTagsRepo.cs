using WebApplicationDailydev.Model;
using System;
using System.Data.SqlClient;

namespace WebApplicationDailydev.Repository
{
    public class NewsTagsRepository
    {
        string connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=DailyDev;Integrated Security=True;Encrypt=True;TrustServerCertificate=true;";


        public void Add(NewsTags newsTags)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "INSERT INTO NewsTags (News_ID, TagID, CreatedDate) VALUES (@News_ID, @TagID, @CreatedDate)";
                var command = new SqlCommand(sql, connection);


                command.Parameters.AddWithValue("@News_ID", newsTags.News_ID);
                command.Parameters.AddWithValue("@TagID", newsTags.TagID);
                command.Parameters.AddWithValue("@CreatedDate", newsTags.CreatedDate);


                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        public IEnumerable<NewsTags> GetAll()
        {
            var newstags = new List<NewsTags>();
            using (var connection = new SqlConnection(connectionString))
            {

                connection.Open();

                string sql = "Select * from NewsTags";
                var command = new SqlCommand(sql, connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    newstags.Add(new NewsTags
                    {
                        NewsTagID = int.Parse(reader["NewsTagID"] + ""),
                        News_ID = int.Parse(reader["News_ID"] + ""),
                        TagID = int.Parse(reader["TagID"] + ""),
                        CreatedDate = (DateTime)reader["CreatedDate"],
                    });
                }
                connection.Close();
            }
            return newstags;

        }

        public NewsTags GetId(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {

                connection.Open();

                string sql = string.Format(@"Select * from NewsTags where NewsTagID = {0}", id);
                var command = new SqlCommand(sql, connection);
                var reader = command.ExecuteReader();

                command.Parameters.AddWithValue("@NewsTagID", id);

                if (reader.Read())
                {
                    return new NewsTags
                    {
                        NewsTagID = int.Parse(reader["NewsTagID"] + ""),
                        News_ID = int.Parse(reader["News_ID"] + ""),
                        TagID = int.Parse(reader["TagID"] + ""),
                        CreatedDate = (DateTime)reader["CreatedDate"],
                    };
                }
                connection.Close();
            }
            return null;
        }

        public void Update(NewsTags newsTags)
        {

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = string.Format(@"Update NewsTags set News_ID = @News_ID,
                                                                 TagID = @TagID,
                                                                 CreatedDate = @CreatedDate,                                                                    
                                                           where NewsTagID = @NewsTagID");
                var command = new SqlCommand(sql, connection);


                command.Parameters.AddWithValue("@NewsTagID", newsTags.NewsTagID);
                command.Parameters.AddWithValue("@News_ID", newsTags.News_ID);
                command.Parameters.AddWithValue("@TagID", newsTags.TagID);
                command.Parameters.AddWithValue("@CreatedDate", newsTags.CreatedDate);

                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = string.Format(@"Delete from NewsTags where NewsTagID = @NewsTagID");
                var command = new SqlCommand(sql, connection);


                command.Parameters.AddWithValue("@NewsTagID", id);

                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}