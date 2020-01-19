using System;

namespace SecretSanta.Data
{
    public class FingerPrintEntityBase : EntityBase
    {
        #nullable disable
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        #nullable enable
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
