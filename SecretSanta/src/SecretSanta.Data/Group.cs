using System.Collections.Generic;

namespace SecretSanta.Data
{
    public class Group : FingerPrintEntityBase
    {
#nullable disable//CS8616 Non-nullable property is uninitialized
        public string Name { get; set; }
#nullable enable
        public List<GroupUser> GroupUsers { get; } = new List<GroupUser>();
    }
}
