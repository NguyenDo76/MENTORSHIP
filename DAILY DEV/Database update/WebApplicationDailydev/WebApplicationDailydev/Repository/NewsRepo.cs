using WebApplicationDailydev.Model;
using System;
using System.Data.SqlClient;

namespace WebApplicationDailydev.Repository
{

    public class NewsRepository
    {        
        string connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=DailyDev;Integrated Security=True;Encrypt=True;TrustServerCertificate=true;";


        public void Add(News news)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = string.Format(@"INSERT INTO News Title, Description, Guid, Link, PubDate, UpdatedDate, ImageURL, SourceID, CategoryID) 
                                                  VALUES (@Title, @Description, @Guid, @Link, @PubDate, @UpdatedDate, @ImageURL, @SourceID, @CategoryID)");
                var command = new SqlCommand(sql, connection);

                command.Parameters.AddWithValue("@Title", news.Title);
                command.Parameters.AddWithValue("@Description", news.Description);
                command.Parameters.AddWithValue("@Guid", news.Guid);
                command.Parameters.AddWithValue("@Link", news.Link);
                command.Parameters.AddWithValue("@PubDate", news.PubDate);
                command.Parameters.AddWithValue("@UpdatedDate", news.UpdatedDate);
                command.Parameters.AddWithValue("@ImageURL", news.ImageURL);
                command.Parameters.AddWithValue("@SourceID", news.SourceID);
                command.Parameters.AddWithValue("@CategoryID", news.CategoryID);


                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public IEnumerable<News> GetAll()
        {
            var news = new List<News>();

            using (var connection = new SqlConnection(connectionString))
            {

                connection.Open();

                string sql = "Select * from News";
                var command = new SqlCommand(sql, connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    news.Add(new News
                    {
                        News_ID = int.Parse(reader["News_ID"] + ""),
                        Title = reader["Title"] + "",
                        Description = reader["Description"] + "",
                        Guid = reader["Guid"] + "",
                        Link = reader["Link"] + "",
                        PubDate = (DateTime)reader["PubDate"],
                        UpdatedDate = (DateTime)reader["UpdatedDate"],
                        ImageURL = reader["ImageURL"] + "",
                        SourceID = int.Parse(reader["SourceID"] + ""),
                        CategoryID = int.Parse(reader["CategoryID"] + ""),
                    });
                }
                connection.Close();
            }
            return news;

        }

        public News GetId(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {

                connection.Open();

                string sql = string.Format(@"Select * from News where News_ID = {0}", id);
                var command = new SqlCommand(sql, connection);
                var reader = command.ExecuteReader();

                command.Parameters.AddWithValue("@News_ID", id);

                if (reader.Read())
                {
                    return new News
                    {
                        News_ID = int.Parse(reader["News_ID"] + ""),
                        Title = reader["Title"] + "",
                        Description = reader["Description"] + "",
                        Guid = reader["Guid"] + "",
                        Link = reader["Link"] + "",
                        PubDate = (DateTime)reader["PubDate"],
                        UpdatedDate = (DateTime)reader["UpdatedDate"],
                        ImageURL = reader["ImageURL"] + "",
                        SourceID = int.Parse(reader["SourceID"] + ""),
                        CategoryID = int.Parse(reader["CategoryID"] + ""),
                    };
                }
                connection.Close();
            }
            return null;
        }

        public void Update(News news)
        {

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = string.Format(@"Update News set Title = @Title,
                                                             Description = @Description,
                                                             Guid = @Guid,
                                                             Link = @Link,
                                                             PubDate = @PubDate,
                                                             UpdatedDate = @UpdatedDate,
                                                             ImageURL = @ImageURL,
                                                             SourceID = @SourceID,
                                                             CategoryID = @Category,
                                                             where News_ID = @News_ID");
                var command = new SqlCommand(sql, connection);


                command.Parameters.AddWithValue("@News_ID", news.News_ID);
                command.Parameters.AddWithValue("@Title", news.Title);
                command.Parameters.AddWithValue("@Description", news.Description);
                command.Parameters.AddWithValue("@Guid", news.Guid);
                command.Parameters.AddWithValue("@Link", news.Link);
                command.Parameters.AddWithValue("@PubDate", news.PubDate);
                command.Parameters.AddWithValue("@UpdatedDate", news.UpdatedDate);
                command.Parameters.AddWithValue("@ImageURL", news.ImageURL);
                command.Parameters.AddWithValue("@SourceID", news.SourceID);
                command.Parameters.AddWithValue("@CategoryID", news.CategoryID);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = string.Format(@"Delete from News where News_ID = @News_ID");
                var command = new SqlCommand(sql, connection);


                command.Parameters.AddWithValue("@News_ID", id);

                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void Upsert(News news)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "Select count (Guid) from Source where Guid = @Guid";
                var command = new SqlCommand(sql, connection);


                command.Parameters.AddWithValue("@Guid", news.Guid);

                var count = (int)command.ExecuteScalar();

                if (count > 0)
                {
                    Update(news);
                }
                else
                {
                    Add(news);
                }
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}