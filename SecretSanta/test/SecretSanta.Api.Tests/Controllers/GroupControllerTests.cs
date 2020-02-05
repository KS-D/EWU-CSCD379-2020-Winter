using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using SecretSanta.Business;
using System;
using SecretSanta.Business.Dto;
using SecretSanta.Business.Services;
using Group = SecretSanta.Data.Group;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class GroupControllerTests : BaseApiControllerTests<Group, GroupInMemoryService, Business.Dto.Group, GroupInput>
    {
        protected override BaseApiController<Group, Business.Dto.Group, GroupInput> CreateController(GroupInMemoryService service)
            => new GroupController(service);

        protected override Group CreateEntity()
            => new Group(Guid.NewGuid().ToString());
    }


    public class GroupInMemoryService : InMemoryEntityService<Group,Business.Dto.Group, GroupInput>, IGroupService
    {

    }
}
