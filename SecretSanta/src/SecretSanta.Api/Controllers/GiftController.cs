using Microsoft.AspNetCore.Mvc;
using SecretSanta.Business.Dto;
using SecretSanta.Business.Services;
using Gift = SecretSanta.Data.Gift;

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiftController : BaseApiController<Gift, Business.Dto.Gift, GiftInput>
    {
        public GiftController(IGiftService giftService)
            : base (giftService)
        { }
    }
}