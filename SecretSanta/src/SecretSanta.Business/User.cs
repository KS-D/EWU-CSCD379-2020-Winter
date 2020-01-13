using System;
using System.Collections.Generic;

namespace SecretSanta.Business
{
    public class User
    {
        public int Id { get; set; }
        private string _FirstName;
        public string FirstName 
        {
            get => _FirstName;
            set => _FirstName = value ?? throw new ArgumentNullException(nameof(value));
        }
        private string _LastName;
        public string LastName 
        {
            get => _LastName;
            set => _LastName = value ?? throw new ArgumentNullException(nameof(value));
        }
        private ICollection<Gift> _Gifts;
        public ICollection<Gift> Gifts 
        { 
            get => _Gifts;
            set => _Gifts = value ?? throw new ArgumentNullException(nameof(value));
        }

        public User(int id, string firstName, string lastName, ICollection<Gift> gifts)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Gifts = gifts;
        }
    }
}