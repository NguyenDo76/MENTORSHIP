namespace WebApplicationDailydev.Model
{
    public class News
    {
        public int News_ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Guid { get; set; }
        public string Link { get; set; }
        public DateTime PubDate { get; set; }        
        public DateTime UpdatedDate { get; set; }
        public string ImageURL {  get; set; }
        public int SourceCategoriesID { get; set; }        

    }
}