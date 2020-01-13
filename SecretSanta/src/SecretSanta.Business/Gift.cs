using System;

namespace SecretSanta.Business
{
    public class Gift
    {
        public int Id { get; }
        private string _Title;
        public string Title 
        { 
            get => _Title;
            set => _Title = value ?? throw new ArgumentNullException(nameof(value));
        }
        private string _Description;
        public string Description 
        { 
            get => _Description;
            set => _Description = value ?? throw new ArgumentNullException(nameof(value));
        }
        private string _Url;
        public string Url 
        {
            get => _Url;
            set => _Url = value ?? throw new ArgumentNullException(nameof(value));
        }

        private User _User;
        public User User 
        { 
            get => _User;
            set => _User = value ?? throw new ArgumentNullException(nameof(value));
        }

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