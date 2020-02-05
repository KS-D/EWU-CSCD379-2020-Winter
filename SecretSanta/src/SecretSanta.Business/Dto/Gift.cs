using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Business.Dto
{
    public class Gift : GiftInput, IEntityBase
    {
        public int Id { get; set; }
    }
}
