using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Business.Dto
{
    public class UserGroupInput
    {
        public int? UserId { get; set; }
        public int? GroupId { get; set; }
        public User? User { get; set; }
        public Group? Group { get; set; }
    }
}
