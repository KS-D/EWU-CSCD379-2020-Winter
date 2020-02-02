using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Business;
using SecretSanta.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecretSanta.Api.Controllers
{
    [Route("api/Group")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private IGroupService GroupService { get; set; }

        public GroupController(IGroupService groupService)
        {
            GroupService = groupService ?? throw new ArgumentNullException(nameof(groupService));
        }

        // GET: api/Group
        [HttpGet]
        public async Task<IEnumerable<Group>> Get()
        {
            return await GroupService.FetchAllAsync();
        }

        // GET: api/Group/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Group>> Get(int id)
        {
            if (await GroupService.FetchByIdAsync(id) is Group group)
            {
                return Ok(group);
            }
            return NotFound();
        }

        // POST: api/Group
        [HttpPost]
        public async Task<Group> Post(Group value)
        {
            return await GroupService.InsertAsync(value);
        }

        // PUT: api/Group/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Group>> Put(int id, Group value)
        {
            if (await GroupService.UpdateAsync(id, value) is Group group)
            {
                return Ok(group);
            }
            return NotFound();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            return await GroupService.DeleteAsync(id);
        }
    }
}