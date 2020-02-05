using Microsoft.AspNetCore.Mvc;
using SecretSanta.Business;
using SecretSanta.Business.Dto;
using SecretSanta.Business.Services;
using User = SecretSanta.Data.User;

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseApiController<User, Business.Dto.User, UserInput>
    {
        public UserController(IUserService userService)
            : base(userService)
        { }
    }
}
