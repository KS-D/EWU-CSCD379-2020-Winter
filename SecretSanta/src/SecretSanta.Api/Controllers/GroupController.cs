using Microsoft.AspNetCore.Mvc;
using SecretSanta.Business.Dto;
using SecretSanta.Business.Services;
using Group = SecretSanta.Data.Group;

namespace SecretSanta.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : BaseApiController<Group, Business.Dto.Group, GroupInput>
    {
        public GroupController(IGroupService groupService) 
            : base(groupService)
        { }
    }
}
