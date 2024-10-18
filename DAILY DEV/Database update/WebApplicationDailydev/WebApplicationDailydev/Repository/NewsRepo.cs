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
using HtmlAgilityPack;


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
    
        private readonly string _connectionString;
        private readonly HttpClient _httpClient;

        public NewsRepository(string connectionString)
        {
            _connectionString = connectionString;
            _httpClient = new HttpClient();
        }
        public async Task<List<SourceCategory>> GetAllLinkRSSAndSourceCategoriesAsync()
        {
            var sourceCategoriesList = new List<SourceCategory>();
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT SourceCategoriesID, LinkRSS FROM SourceCategories";
                using (var command = new SqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        sourceCategoriesList.Add(new SourceCategory
                        {
                            SourceCategoriesID = reader.GetInt32(0),
                            LinkRSS = reader.GetString(1)
                        });
                    }
                }
            }
            return sourceCategoriesList;
        }

        public async Task<string> FetchRSSFeedAsync(string rssUrl)
        {
            //return await _httpClient.GetStringAsync(rssUrl);
            var request = new HttpRequestMessage(HttpMethod.Get, rssUrl);
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to fetch RSS feed from {rssUrl}: {response.StatusCode}");
                return null;
            }

            //return await response.Content.ReadAsStringAsync();
            var content = await response.Content.ReadAsStringAsync();

            // Log nội dung RSS nhận được
            Console.WriteLine($"RSS content from {rssUrl}: {content}");

            return content;
        }

        public List<News> ProcessRSSData(string rssFeedContent, int sourceCategoriesID)
        {
            var newsList = new List<News>();
            var rssXml = XDocument.Parse(rssFeedContent);

            foreach (var item in rssXml.Descendants("item"))
            {
                try
                {
                    var pubDateString = item.Element("pubDate")?.Value
                                 ?? item.Element("{http://purl.org/dc/elements/1.1/}date")?.Value;
                    DateTime pubDate;

                    // Sử dụng DateTime.ParseExact để xử lý định dạng ngày
                    string[] formats = { "ddd, dd MMM yyyy HH:mm:ss 'GMT'K", "r", "ddd, dd MMM yyyy HH:mm:ss zzz" };
                    if (DateTime.TryParseExact(pubDateString, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AdjustToUniversal, out pubDate))
                    {
                        var title = item.Element("title")?.Value;
                        var description = item.Element("description")?.Value;
                        var link = item.Element("link")?.Value;
                        var guid = item.Element("guid")?.Value;

                        //Kiểm tra nếu sử dụng media:content hoặc media: thumbnail cho hình ảnh
                        var imageUrl = item.Element("enclosure")?.Attribute("url")?.Value
                                    ?? item.Element("{http://search.yahoo.com/mrss/}thumbnail")?.Attribute("url")?.Value
                                    ?? item.Element("{http://search.yahoo.com/mrss/}content")?.Attribute("url")?.Value
                                    ?? item.Element("{http://purl.org/rss/1.0/modules/content/}encoded")?.Value;



                        var news = new News
                        {
                            Title = title,
                            Description = description,
                            Link = link,
                            Guid = guid,
                            PubDate = pubDate,
                            ImageURL = imageUrl,
                            SourceCategoriesID = sourceCategoriesID
                        };
                        newsList.Add(news);
                    }
                    else
                    {
                        Console.WriteLine($"Lỗi khi parse pubDate: {pubDateString}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing item: {ex.Message}");
                }
            }
            return newsList;
        }
        //------------------------------


        //------------------------------

        // Chỉ thêm các news với GUID không trùng lặp
        public async Task AddUniqueNewsAsync(List<News> newsItems)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                foreach (var news in newsItems)
                {
                    // Kiểm tra nếu GUID đã tồn tại
                    string checkGuidQuery = "SELECT COUNT(1) FROM News WHERE Guid = @Guid";
                    using (var checkCommand = new SqlCommand(checkGuidQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@Guid", news.Guid);
                        var count = (int)await checkCommand.ExecuteScalarAsync();

                        // Nếu GUID không tồn tại, chèn dữ liệu mới
                        if (count == 0)
                        {
                            string insertQuery = @"INSERT INTO News (Title, Description, Link, Guid, PubDate, UpdatedDate, ImageURL, SourceCategoriesID) 
                                               VALUES (@Title, @Description, @Link, @Guid, @PubDate, @UpdatedDate, @ImageURL, @SourceCategoriesID)";
                            using (var insertCommand = new SqlCommand(insertQuery, connection))
                            {
                                insertCommand.Parameters.AddWithValue("@Title", news.Title);
                                insertCommand.Parameters.AddWithValue("@Description", news.Description);
                                insertCommand.Parameters.AddWithValue("@Link", news.Link);
                                insertCommand.Parameters.AddWithValue("@Guid", news.Guid);
                                insertCommand.Parameters.AddWithValue("@PubDate", news.PubDate);
                                insertCommand.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                                insertCommand.Parameters.AddWithValue("@ImageURL", (object)news.ImageURL ?? DBNull.Value);
                                insertCommand.Parameters.AddWithValue("@SourceCategoriesID", news.SourceCategoriesID);

                                await insertCommand.ExecuteNonQueryAsync();
                            }
                        }
                    }
                }
            }
        }

    }
}
