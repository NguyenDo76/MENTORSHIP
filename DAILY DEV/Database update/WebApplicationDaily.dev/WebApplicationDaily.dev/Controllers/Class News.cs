namespace WebApplicationDaily.dev.Controllers
{
    public class News
    {
        public int News_ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string Guid { get; set; }
        public DateTime PubDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string ImageURL { get; set; }
        public int SourceID { get; set; }
        public int CategoryID { get; set; }
    }
}