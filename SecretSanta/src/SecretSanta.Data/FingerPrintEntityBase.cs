using System;

namespace SecretSanta.Data
{
    public class FingerPrintEntityBase : EntityBase
    {
#nullable disable//CS8616 Non-nullable property is uninitialized
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
#nullable enable
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
