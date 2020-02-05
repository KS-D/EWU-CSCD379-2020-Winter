using System.Collections.Generic;

namespace SecretSanta.Business.Dto
{
    public class UserInput
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? SantaId { get; set; }
        public User? Santa { get; set; }
        public IList<Gift>? Gifts { get; set; }
        public IList<UserGroup>? UserGroups { get; set; }
    }
}