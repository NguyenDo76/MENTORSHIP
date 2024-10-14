using System;
using WebApplicationDailydev.Model;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration.UserSecrets;


namespace WebApplicationDailydev.Repository
{
    public class PostRepository
    {
        string connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=DailyDev;Integrated Security=True;Encrypt=True;TrustServerCertificate=true;";
        public void Add(Post post)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = string.Format(@"Insert into Post UserID, News_ID, CategoryID, View, Like, Comment, Bookmark 
                                                        values (@UserID, @News_ID, @CategoryID, @View, @Like, @Comment, @Bookmark)");
                var command = new SqlCommand(sql, connection);

                command.Parameters.AddWithValue("@UserID", post.UserID);
                command.Parameters.AddWithValue("@News_ID", post.News_ID);
                command.Parameters.AddWithValue("@CategoryID", post.CategoryID);
                command.Parameters.AddWithValue("@View", post.View);
                command.Parameters.AddWithValue("@Like", post.Like);
                command.Parameters.AddWithValue("@Comment", post.Comment);
                command.Parameters.AddWithValue("@Bookmarks", post.Bookmark);

                command.ExecuteNonQuery();
                connection.Close();

            }

        }

        public IEnumerable<Post> GetAll()
        {
            var post = new List<Post>();

            using (var connection = new SqlConnection(connectionString))
            {

                connection.Open();

                string sql = "Select * from Post";
                var command = new SqlCommand(sql, connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    post.Add(new Post
                    {
                        PostID = int.Parse(reader["PostID"] + ""),
                        UserID = int.Parse(reader["UserID"] + ""),
                        News_ID = int.Parse(reader["News_ID"] + ""),
                        CategoryID = int.Parse(reader["CategoryID"] + ""),
                        View = int.Parse(reader["View"] + ""),
                        Like = int.Parse(reader["Like"] + ""),
                        Bookmark = int.Parse(reader["Bookmark"] + ""),
                        
                    });
                }
                connection.Close();
            }
            return post;

        }

        public Post GetId(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {

                connection.Open();

                string sql = string.Format(@"Select * from Post where PostID = {0}", id);
                var command = new SqlCommand(sql, connection);
                var reader = command.ExecuteReader();

                command.Parameters.AddWithValue("@PostID", id);

                if (reader.Read())
                {
                    return new Post
                    {
                        PostID = int.Parse(reader["PostID"] + ""),
                        UserID = int.Parse(reader["UserID"] + ""),
                        News_ID = int.Parse(reader["News_ID"] + ""),
                        CategoryID = int.Parse(reader["CategoryID"] + ""),
                        View = int.Parse(reader["View"] + ""),
                        Like = int.Parse(reader["Like"] + ""),
                        Bookmark = int.Parse(reader["Bookmark"] + ""),
                    };
                }
                connection.Close();
            }
            return null;
        }

        public void Update(Post post)
        {

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = string.Format(@"Update Post set UserID = @UserID,
                                                             News_ID = @News_ID,
                                                             CategoryID = @CategoryID,
                                                             View = @View,
                                                             Like = @Like,
                                                             Bookmark = @Bookmark
                                                            where PostID = @PostID");
                var command = new SqlCommand(sql, connection);


                command.Parameters.AddWithValue("@PostID", post.PostID);
                command.Parameters.AddWithValue("@UserID", post.UserID);
                command.Parameters.AddWithValue("@News_ID", post.News_ID);
                command.Parameters.AddWithValue("@CategoryID", post.CategoryID);
                command.Parameters.AddWithValue("@View", post.View);
                command.Parameters.AddWithValue("@Like", post.Like);
                command.Parameters.AddWithValue("@Comment", post.Comment);
                command.Parameters.AddWithValue("@Bookmarks", post.Bookmark);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = string.Format(@"Delete from Post where PostID = @PostID");
                var command = new SqlCommand(sql, connection);


                command.Parameters.AddWithValue("@PostID", id);

                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void Upsert(Post post)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "Select count (News_ID) from Source where News_ID = @News_ID";
                var command = new SqlCommand(sql, connection);


                command.Parameters.AddWithValue("@News_ID", post.News_ID);

                var count = (int)command.ExecuteScalar();

                if (count > 0)
                {
                    Update(post);
                }
                else
                {
                    Add(post);
                }
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
