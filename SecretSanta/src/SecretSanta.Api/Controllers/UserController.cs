using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Business;
using SecretSanta.Data;

namespace SecretSanta.Api.Controllers
{

    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService UserService { get; }

        public UserController(IUserService userService)
        {
            UserService = userService ?? throw new System.ArgumentNullException(nameof(userService));
        }

        // GET: api/User
        [HttpGet]
        public async Task<IEnumerable<User>> Get()
        {
            List<User> users = await UserService.FetchAllAsync();
            return users;
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> Get(int id)
        {
            if (await UserService.FetchByIdAsync(id) is User user)
            {
                return Ok(user);
            }
            return NotFound();
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<User>> Post(User value)
        {
            return await UserService.InsertAsync(value);
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> Put(int id, User value)
        {
            if (await UserService.UpdateAsync(id, value) is User user)
            {
                return user;
            }
            return NotFound();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            return await UserService.DeleteAsync(id);
        }
    }
}