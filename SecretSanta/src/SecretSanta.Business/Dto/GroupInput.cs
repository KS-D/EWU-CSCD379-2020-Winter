using System.Collections.Generic;

namespace SecretSanta.Business.Dto
{
    public class GroupInput
    {
        public string? Title { get; set; }
        public IList<UserGroup>? UserGroups { get; } = new List<UserGroup>();

    }
}