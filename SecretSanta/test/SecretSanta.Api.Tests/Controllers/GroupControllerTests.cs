using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using System;
using SecretSanta.Business.Dto;
using SecretSanta.Business.Services;
using Group = SecretSanta.Data.Group;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class GroupControllerTests : BaseApiControllerTests<Group, IGroupService, Business.Dto.Group, GroupInput>
    {
        protected override BaseApiController<Business.Dto.Group, GroupInput> CreateController(IGroupService service)
            => new GroupController(service);

        protected override Business.Dto.Group CreateEntity() => new Business.Dto.Group
        {
            Title = Guid.NewGuid().ToString(),
            Id = new Random().Next()
        };
    }
}
