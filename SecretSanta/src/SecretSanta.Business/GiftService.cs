
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SecretSanta.Data;

namespace SecretSanta.Business
{
    // TODO override the fetch methods to also fetch the gifts user
    public class GiftService : EntityService<Gift>, IGiftService
    {
        public GiftService(ApplicationDbContext applicationDbContext, IMapper mapper) : base(applicationDbContext, mapper)
        {
        }

        public override async Task<Gift> FetchByIdAsync(int id)
        {
            return await ApplicationDbContext.Gifts.Include(g => g.User)
                .SingleOrDefaultAsync(item => item.Id == id);
 
        }
   }
}
