using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using System;
using SecretSanta.Business.Dto;
using SecretSanta.Business.Services;
using SecretSanta.Data;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class UserControllerTests : BaseApiControllerTests<Data.User, UserInMemoryService, Business.Dto.User, UserInput>
    {
        protected override BaseApiController<Data.User, Business.Dto.User, UserInput> CreateController(UserInMemoryService service)
            => new UserController(service);

        protected override Data.User CreateEntity()
            => new Data.User(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
    }

    public class UserInMemoryService : InMemoryEntityService<Data.User, Business.Dto.User, UserInput>, IUserService
    {

    }
}
