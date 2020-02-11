using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using System;
using SecretSanta.Business.Dto;
using SecretSanta.Business.Services;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class UserControllerTests : BaseApiControllerTests< IUserService, User, UserInput>
    {
        protected override BaseApiController<User, UserInput> CreateController(IUserService service)
            => new UserController(service);

        protected override User CreateDto()
            => new User
            {
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                Id = new Random().Next(),
                SantaId = new Random().Next()
            };

    }

}
