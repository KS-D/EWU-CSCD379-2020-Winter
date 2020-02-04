using Microsoft.AspNetCore.Mvc;
using SecretSanta.Business;
using SecretSanta.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace SecretSanta.Api.Controllers
{
    [Route("api/Gift")]
    [ApiController]
    public class GiftController : ControllerBase
    {
        private IGiftService GiftService { get; }

        public GiftController(IGiftService giftService)
        {
            GiftService = giftService ?? throw new System.ArgumentNullException(nameof(giftService));
        }

        // GET: api/Gift
        [HttpGet]
        public async Task<IEnumerable<Gift>> Get()
        {
            List<Gift> gifts = await GiftService.FetchAllAsync();
            return gifts;
        }

        // GET: api/Gift/5
        [HttpGet("{id}")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Gift>> Get(int id)
        {
            if (await GiftService.FetchByIdAsync(id) is Gift gift)
            {
                return Ok(gift);
            }
            return NotFound();
        }

        // POST: api/Gift
        [HttpPost]
        public async Task<Gift> Post(Gift value)
        {
            return await GiftService.InsertAsync(value);
        }

        // PUT: api/Gift/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Gift>> Put(int id, Gift value)
        {
            if (await GiftService.UpdateAsync(id, value) is Gift gift)
            {
                return Ok(gift);
            }
            return NotFound();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            if (await GiftService.DeleteAsync(id))
            {
                return Ok();
            }

            return NotFound();
        }
    }
}