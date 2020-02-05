namespace SecretSanta.Business.Dto
{
    public class User : UserInput, IEntityBase
    {
        public int Id { get; set; }
    }
}