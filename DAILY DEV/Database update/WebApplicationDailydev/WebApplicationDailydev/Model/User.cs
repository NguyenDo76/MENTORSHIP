namespace WebApplicationDailydev.Model
{
    public class User
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string Email { get; set; }        
        public DateTime JoinedDate { get; set; }
        public DateTime LastedSignOut { get; set; }
    }
}
