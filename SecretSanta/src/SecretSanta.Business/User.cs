using System.Collections.Generic;

namespace SecretSanta.Business
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Gift> Gifts { get; set; }

        public User(int id, string firstName, string lastName, ICollection<Gift> gifts)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Gifts = gifts;
        }
    }
}