
using AutoMapper;
using SecretSanta.Data;

namespace SecretSanta.Business
{
    // TODO override the fetch methods to also fetch the gifts user
    public class GiftService : EntityService<Gift>, IGiftService
    {
        public GiftService(ApplicationDbContext applicationDbContext, IMapper mapper) : base(applicationDbContext, mapper)
        {
        }
    }
}
