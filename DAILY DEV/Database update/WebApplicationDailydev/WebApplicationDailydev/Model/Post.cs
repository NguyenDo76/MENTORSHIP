namespace WebApplicationDailydev.Model
{
    public class Post
    {
        public int PostID { get; set; }
        public int UserID { get; set; }
        public int News_ID { get; set; }
        public int CategoryID { get; set; }
        public int View { get; set;}
        public int Like {  get; set; }
        public int Comment { get; set; }
        public int Bookmark { get; set; }
    }
}
