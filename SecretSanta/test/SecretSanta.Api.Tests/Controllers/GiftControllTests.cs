using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using System;
using SecretSanta.Business.Dto;
using SecretSanta.Business.Services;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class GiftControllTests : BaseApiControllerTests<Data.Gift, IGiftService, Business.Dto.Gift, GiftInput>
    {
        protected override BaseApiController<Business.Dto.Gift, GiftInput> CreateController(IGiftService service)
            => new GiftController(service);

        protected override Business.Dto.Gift CreateEntity()
            => new Business.Dto.Gift
            {
                Title = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Id = new Random().Next(),
                Url = Guid.NewGuid().ToString(),
                UserId = new Random().Next()
            };
    }
}
