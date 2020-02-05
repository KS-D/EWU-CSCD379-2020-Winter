using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using System;
using SecretSanta.Business.Dto;
using SecretSanta.Business.Services;
using Gift = SecretSanta.Data.Gift;
using User = SecretSanta.Data.User;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class GiftControllTests : BaseApiControllerTests<Gift, GiftInMemoryService, Business.Dto.Gift, GiftInput>
    {
        protected override BaseApiController<Gift, Business.Dto.Gift, GiftInput> CreateController(GiftInMemoryService service)
            => new GiftController(service);

        protected override Gift CreateEntity()
            => new Gift(Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                new User(Guid.NewGuid().ToString(), Guid.NewGuid().ToString()));
    }

    public class GiftInMemoryService : InMemoryEntityService<Gift, Business.Dto.Gift, GiftInput>, IGiftService
    {

    }
}
