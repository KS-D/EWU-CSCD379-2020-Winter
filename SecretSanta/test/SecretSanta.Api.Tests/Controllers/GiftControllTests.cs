using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using System;
using SecretSanta.Business.Dto;
using SecretSanta.Business.Services;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class GiftControllTests : BaseApiControllerTests< IGiftService, Business.Dto.Gift, GiftInput>
    {
        protected override BaseApiController<Gift, GiftInput> CreateController(IGiftService service)
            => new GiftController(service);

        protected override Gift CreateEntity()
            => new Gift
            {
                Title = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Id = new Random().Next(),
                Url = Guid.NewGuid().ToString(),
                UserId = new Random().Next()
            };
    }
}
