namespace SecretSanta.Business
{
    public class Gift
    {
        public int Id { get; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public User User { get; set; }

        public Gift(int id, string title, string desctription, string url, User user)
        {
            Id = id;
            Title = title;
            Description = desctription;
            Url = url;
            User = user;
        }
    }
}