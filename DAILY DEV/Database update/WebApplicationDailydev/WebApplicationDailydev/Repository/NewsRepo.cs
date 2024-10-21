using WebApplicationDailydev.Model;
using System.Data;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using Quartz;
using System.Threading;
using System.Threading.Tasks;
using WebApplicationDailydev.Repository;
using Quartz.Spi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;



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

                string sql = string.Format(@"INSERT INTO News (Title, Description, Guid, Link, PubDate, UpdatedDate, ImageURL, SourceCategoriesID) 
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
                                                             SourceCategoriesID = @SourceCategoriesID
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
                string sql = "Select count(*) from News where Guid = @Guid";
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

        public NewsRepository(string connectionString, HttpClient httpClient)
        {
            _connectionString = connectionString;
            _httpClient = new HttpClient();
        }
        //private readonly IHttpClientFactory _httpClientFactory;

        //public NewsRepository(string connectionString, IHttpClientFactory httpClientFactory)
        //{
        //    _connectionString = connectionString;
        //    _httpClientFactory = httpClientFactory;
        //}

        public async Task FetchAndSaveNewsFromRSSAsync(CancellationToken cancellationToken)
        {
            var sourceCategories = await GetAllSourceCategoriesAsync(cancellationToken);

            int batchSize = 5; // Kích thước batch

            for (int i = 0; i < sourceCategories.Count(); i += batchSize)
            {
                var batchCategories = sourceCategories.Skip(i).Take(batchSize);
                var tasks = batchCategories.Select(async sourceCategory =>
                {
                    var rssXml = await FetchRssContentAsync(sourceCategory.LinkRSS, cancellationToken);
                    if (rssXml != null)
                    {
                        await ParseAndSaveRssAsync(rssXml, sourceCategory.SourceCategoriesID);
                    }
                });

                await Task.WhenAll(tasks);
            }
        }

        private async Task<List<SourceCategory>> GetAllSourceCategoriesAsync(CancellationToken cancellationToken)
        {
            var sourceCategories = new List<SourceCategory>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                string sql = "SELECT * FROM SourceCategories";
                var command = new SqlCommand(sql, connection);
                var reader = await command.ExecuteReaderAsync(cancellationToken);

                while (await reader.ReadAsync(cancellationToken))
                {
                    sourceCategories.Add(new SourceCategory
                    {
                        SourceCategoriesID = int.Parse(reader["SourceCategoriesID"].ToString()),
                        SourceID = int.Parse(reader["SourceID"].ToString()),
                        CategoryID = int.Parse(reader["CategoryID"].ToString()),
                        LinkRSS = reader["LinkRSS"].ToString(),
                    });
                }
                connection.Close();
            }

            return sourceCategories;
        }

        private async Task<XDocument> FetchRssContentAsync(string rssUrl, CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync(rssUrl, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var rssContent = await response.Content.ReadAsStringAsync();
                return XDocument.Parse(rssContent);
            }
            return null;
        }

        public async Task ParseAndSaveRssAsync(XDocument rssXml, int categoryId)
        {
            var items = rssXml.Descendants("item");

            foreach (var item in items)
            {
                var newItem = new News
                {
                    Title = GetElementValue(item, "title"),
                    Link = GetElementValue(item, "link"),
                    Guid = GetElementValue(item, "guid"),
                    PubDate = ParseRssDate(GetElementValue(item, "pubDate")),
                    UpdatedDate = DateTime.Now,
                    ImageURL = GetImageUrl(item) ?? "Null",
                    SourceCategoriesID = categoryId,
                    Description = GetElementValue(item, "description") ?? "Null"
                };

                Upsert(newItem);
            }
        }

        private string GetElementValue(XElement item, string elementName)
        {
            return item.Element(elementName)?.Value;
        }

        private string GetImageUrl(XElement item)
        {
            // Kiểm tra xem có thẻ 'enclosure' không
            var enclosure = item.Element("enclosure")?.Attribute("url")?.Value;
            if (!string.IsNullOrEmpty(enclosure))
            {
                return enclosure;
            }

            // Nếu không có thẻ 'enclosure', kiểm tra trong 'description'
            var description = item.Element("description")?.Value;
            if (!string.IsNullOrEmpty(description))
            {
                // Sử dụng Regex để tìm thẻ <img> và lấy giá trị src
                var match = Regex.Match(description, "<img[^>]+src\\s*=\\s*['\"](?<url>[^'\"]+)['\"][^>]*>");
                if (match.Success)
                {
                    return match.Groups["url"].Value;
                }
            }

            // Nếu không có hình ảnh nào được tìm thấy, trả về null
            return null;
        }

        public DateTime ParseRssDate(string dateString)
        {
            // Xử lý chuỗi ngày giờ và các định dạng có thể
            string[] formats = {
        "ddd, dd MMM yyyy HH:mm:ss K",         // Định dạng có múi giờ
        "ddd, dd MMM yyyy HH:mm:ss zzz",       // Định dạng với zzz
        "ddd, dd MMM yyyy HH:mm:ss",           // Định dạng cơ bản không có múi giờ
        "ddd, dd MMM yy HH:mm:ss K",           // Định dạng năm 2 chữ số với K
        "ddd, dd MMM yyyy HH:mm:ss 'GMT'K",    // Định dạng với GMT và K
        "ddd, dd MMM yyyy HH:mm:ss 'GMT'zzz",  // Định dạng với GMT và zzz
        "ddd, dd MMM yyyy HH:mm:ss +hh:mm",     // Định dạng với múi giờ
        "ddd, dd MMM yyyy HH:mm:ss -hh:mm",     // Định dạng với múi giờ âm
            };

            // Nếu dateString có chứa 'GMT', ta sẽ xóa nó
            dateString = dateString.Replace("GMT", "").Trim();

            // Chuyển đổi cú pháp của múi giờ từ +7 thành +07:00
            if (dateString.EndsWith("+7"))
            {
                dateString = dateString.Replace("+7", "+07:00");
            }
            if (dateString.EndsWith("+07"))
            {
                dateString = dateString.Replace("+07", "+07:00");
            }
            else if (dateString.EndsWith("+0700"))
    {
                dateString = dateString.Replace("+0700", "+07:00");
            }

            // Phân tích cú pháp ngày giờ
            if (DateTimeOffset.TryParseExact(dateString, formats, null, System.Globalization.DateTimeStyles.None, out var dateTimeOffset))
            {
                return dateTimeOffset.UtcDateTime;
            }
            throw new FormatException($"Unable to parse date: {dateString}");
        }
        public class RssFetchJob : IJob
        {
            private readonly NewsRepository _newsRepository;

            public RssFetchJob(NewsRepository newsRepository)
            {
                _newsRepository = newsRepository;
            }

            public async Task Execute(IJobExecutionContext context)
            {
                CancellationToken cancellationToken = context.CancellationToken;
                try
                {
                    await _newsRepository.FetchAndSaveNewsFromRSSAsync(cancellationToken);
                    Console.WriteLine("RSS data fetched and saved successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error fetching RSS data: {ex.Message}");
                }
            }
        }
        public class JobSchedule
        {
            public Type JobType { get; }
            public string CronExpression { get; }

            public JobSchedule(Type jobType, string cronExpression)
            {
                JobType = jobType;
                CronExpression = cronExpression;
            }
        }
        public class SingletonJobFactory : IJobFactory
        {
            private readonly IServiceProvider _serviceProvider;

            public SingletonJobFactory(IServiceProvider serviceProvider)
            {
                _serviceProvider = serviceProvider;
            }

            public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
            {
                return _serviceProvider.GetRequiredService(bundle.JobDetail.JobType) as IJob;
            }

            public void ReturnJob(IJob job) { }
        }
        public class QuartzHostedService : IHostedService
        {
            private readonly ISchedulerFactory _schedulerFactory;
            private readonly IJobFactory _jobFactory;
            private readonly IEnumerable<JobSchedule> _jobSchedules;
            private IScheduler _scheduler;

            public QuartzHostedService(
                ISchedulerFactory schedulerFactory,
                IJobFactory jobFactory,
                IEnumerable<JobSchedule> jobSchedules)
            {
                _schedulerFactory = schedulerFactory;
                _jobFactory = jobFactory;
                _jobSchedules = jobSchedules;
            }

            public async Task StartAsync(CancellationToken cancellationToken)
            {
                _scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
                _scheduler.JobFactory = _jobFactory;

                foreach (var jobSchedule in _jobSchedules)
                {
                    var job = CreateJob(jobSchedule);
                    var trigger = CreateTrigger(jobSchedule);

                    await _scheduler.ScheduleJob(job, trigger, cancellationToken);
                }

                await _scheduler.Start(cancellationToken);
            }

            public async Task StopAsync(CancellationToken cancellationToken)
            {
                if (_scheduler != null)
                {
                    await _scheduler.Shutdown(cancellationToken);
                }
            }

            private static IJobDetail CreateJob(JobSchedule schedule)
            {
                var jobType = schedule.JobType;
                return JobBuilder
                    .Create(jobType)
                    .WithIdentity(jobType.FullName)
                    .WithDescription(jobType.Name)
                    .Build();
            }

            private static ITrigger CreateTrigger(JobSchedule schedule)
            {
                return TriggerBuilder
                    .Create()
                    .WithIdentity($"{schedule.JobType.FullName}.trigger")
                    .WithCronSchedule(schedule.CronExpression)
                    .WithDescription(schedule.CronExpression)
                    .Build();
            }
        }
        


    }
}
