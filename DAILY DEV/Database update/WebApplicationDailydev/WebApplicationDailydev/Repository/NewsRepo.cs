using WebApplicationDailydev.Model;
using System;
using System.Data.SqlClient;
using System.Xml;

using System.Collections.Generic;
using System.Net.Http;
using System.ServiceModel.Syndication;


using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using System.Data;
using System.Xml.Linq;

using Polly;
using Polly.Retry;
using System;
using System.Net.Http;
using System.Threading.Tasks;


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

                string sql = string.Format(@"INSERT INTO News Title, Description, Guid, Link, PubDate, UpdatedDate, ImageURL, SourceCategoriesID) 
                                                  VALUES (@Title, @Description, @Guid, @Link, @PubDate, @UpdatedDate, @ImageURL, @SourceCategoriesID)");
                var command = new SqlCommand(sql, connection);

                command.Parameters.AddWithValue("@Title", news.Title);
                command.Parameters.AddWithValue("@Description", news.Description);
                command.Parameters.AddWithValue("@Guid", news.Guid);
                command.Parameters.AddWithValue("@Link", news.Link);
                command.Parameters.AddWithValue("@PubDate", news.PubDate);
                command.Parameters.AddWithValue("@UpdatedDate", news.UpdatedDate);
                command.Parameters.AddWithValue("@ImageURL", news.ImageURL);
                command.Parameters.AddWithValue("@SourceCategoriesID", news.SourceCategoriesID);



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
                        SourceCategoriesID = int.Parse(reader["SourceCategoriesID"] + ""),
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
                        SourceCategoriesID = int.Parse(reader["SourceCategoriesID"] + ""),
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
                                                             SourceCategoriesID = @SourceCategoriesID,
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
                command.Parameters.AddWithValue("@SourceCategoriesID", news.SourceCategoriesID);
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

        //----------------------------------------------------

        public async Task<List<string>> GetAllLinkRSSAsync()
        {
            var linkRSSList = new List<string>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT LinkRSS FROM SourceCategories";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        linkRSSList.Add(reader.GetString(0));
                    }
                }
            }
            return linkRSSList;
        }

        // Thêm dữ liệu vào bảng News
        public async Task AddNewsAsync(News news)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO News (Title, Description, Link, Guid, PubDate, UpdatedDate, ImageURL, SourceCategoriesID)
                             VALUES (@Title, @Description, @Link, @Guid, @PubDate, @UpdatedDate, @ImageURL, @SourceCategoriesID)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Title", news.Title);
                cmd.Parameters.AddWithValue("@Description", news.Description);
                cmd.Parameters.AddWithValue("@Link", news.Link);
                cmd.Parameters.AddWithValue("@Guid", news.Guid);
                cmd.Parameters.AddWithValue("@PubDate", news.PubDate);
                cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now); // Cập nhật ngày hiện tại
                cmd.Parameters.AddWithValue("@ImageURL", news.ImageURL);
                cmd.Parameters.AddWithValue("@SourceCategoriesID", news.SourceCategoriesID);

                conn.Open();
                await cmd.ExecuteNonQueryAsync();
            }
        }

        // Phân tích dữ liệu từ RSS feed
        public async Task<List<News>> FetchRSSDataAsync(string rssUrl, int sourceCategoriesID)
        {
            var newsList = new List<News>();
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetStringAsync(rssUrl);
                var rssXml = XDocument.Parse(response);

                foreach (var item in rssXml.Descendants("item"))
                {
                    var pubDateString = item.Element("pubDate")?.Value;
                    DateTime pubDate;

                    // Sử dụng DateTime.ParseExact để chuyển đổi định dạng ngày tháng
                    string[] formats = { "ddd, dd MMM yyyy HH:mm:ss 'GMT'K", "r", "ddd, dd MMM yyyy HH:mm:ss zzz" };
                    if (DateTime.TryParseExact(pubDateString, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AdjustToUniversal, out pubDate))
                    {
                        var news = new News
                        {
                            Title = item.Element("title")?.Value,
                            Description = item.Element("description")?.Value,
                            Link = item.Element("link")?.Value,
                            Guid = item.Element("guid")?.Value,
                            PubDate = pubDate,
                            ImageURL = item.Element("enclosure")?.Attribute("url")?.Value, // nếu RSS có image
                            SourceCategoriesID = sourceCategoriesID
                        };
                        newsList.Add(news);
                    }
                    else
                    {
                        // Handle error when parsing date
                        Console.WriteLine($"Unable to parse pubDate: {pubDateString}");
                    }
                }
            }
            return newsList;
        }
        
            private readonly HttpClient _httpClient;
            private readonly IAsyncPolicy<HttpResponseMessage> _retryPolicy;

            public NewsRepository()
            {
                _httpClient = new HttpClient();

                // Cấu hình Polly để retry 3 lần khi có lỗi
                _retryPolicy = Policy
                    .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (response, timespan, retryCount, context) =>
                        {
                            Console.WriteLine($"Request failed. Waiting {timespan} before next retry. Retry attempt {retryCount}");
                        });
            }

            public async Task<string> FetchRSSFeedAsync(string rssUrl)
            {
                // Sử dụng retry policy cho HTTP request
                HttpResponseMessage response = await _retryPolicy.ExecuteAsync(() => _httpClient.GetAsync(rssUrl));

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Failed to fetch RSS feed. Status code: {response.StatusCode}");
                }

                return await response.Content.ReadAsStringAsync();
            }
        
    }
}