using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using System;
using SecretSanta.Business.Dto;
using SecretSanta.Business.Services;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class GroupControllerTests : BaseApiControllerTests<IGroupService, Group, GroupInput>
    {
        protected override BaseApiController<Group, GroupInput> CreateController(IGroupService service)
            => new GroupController(service);

        protected override Group CreateEntity() => new Group
        {
            Title = Guid.NewGuid().ToString(),
            Id = new Random().Next()
        };
    }
}
