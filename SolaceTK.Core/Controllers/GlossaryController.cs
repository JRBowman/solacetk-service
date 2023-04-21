using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolaceTK.Core.Contexts;
using System;
using System.Threading.Tasks;

namespace SolaceTK.Core.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class GlossaryController : ControllerBase
    {
        //private GlossaryContext context;

        public GlossaryController(GlossaryContext _context)
        {
            //context = _context;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            //return Ok(context.Index);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] string type, [FromQuery] Guid? id)
        {
            //GlossaryIndex result = null;
            //if (id != null)
            //{
            //    result = await context.Index.FirstOrDefaultAsync(x => x.Id == id);
            //}

            //if (type != null)
            //{
            //    result = await context.Index.FirstOrDefaultAsync(x => x.ApiType == type);
            //}

            //if (result == null) return NotFound();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Register(object registration)
        {
            //if (registration.Id == Guid.Empty)
            //{
            //    registration.Id = Guid.NewGuid();
            //}

            //if (string.IsNullOrEmpty(registration.Address))
            //{
            //    return BadRequest("The Address field is Required for Registration.");
            //}

            //if (!Uri.IsWellFormedUriString(registration.Address, UriKind.RelativeOrAbsolute))
            //{
            //    return BadRequest("The Address provided is not a well-formed URI.");
            //}

            //var result = await context.Index.AddAsync(registration);

            //if (result.State != EntityState.Added)
            //{
            //    return BadRequest("Adding Registration to the Database failed.");
            //}

            //await context.SaveChangesAsync();

            return Created($"{this.Url}", registration);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Guid id, object registration)
        {

            return Ok(registration);
        }

        [HttpDelete]
        public async Task<IActionResult> Unregister(Guid id)
        {
            //var registration = await context.Index.FirstOrDefaultAsync(x => x.Id == id);
            //if (registration == null) return NotFound();
            
            //context.Index.Remove(registration);

            //var result = await context.SaveChangesAsync();

            //if (result == 0) return BadRequest("Failed to delete the registration - please try again.");

            return NoContent();
        }

    }
}
