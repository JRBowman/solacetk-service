using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolaceTK.Core.Contexts;
using SolaceTK.Core.Models.Behavior;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SolaceTK.Core.Controllers.Events
{

    [Route("api/v1/Events/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly BehaviorContext _context;

        public MessagesController(BehaviorContext context)
        {
            _context = context;
        }

        // GET: api/BehaviorMessage
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BehaviorMessage>>> GetBehaviorMessages()
        {
            return await _context.Messages.Include(x => x.Data).ToListAsync();
        }

        // GET: api/BehaviorMessage/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BehaviorMessage>> GetBehaviorMessage(int id)
        {
            var ev = await _context.Messages.Include(x => x.Data).FirstOrDefaultAsync(x => x.Id == id);

            if (ev == null)
            {
                return NotFound();
            }

            return ev;
        }

        // POST: api/BehaviorMessage
        [HttpPost]
        public async Task<ActionResult<BehaviorMessage>> PostBehaviorMessage(BehaviorMessage ev)
        {
            _context.Messages.Add(ev);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBehaviorMessage", new { id = ev.Id }, ev);
        }

        // PUT: api/BehaviorMessage/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBehaviorMessage(int id, BehaviorMessage ev)
        {
            if (id != ev.Id)
            {
                return BadRequest();
            }

            _context.Entry(ev).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BehaviorMessageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(ev);
        }

        // DELETE: api/BehaviorMessage/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<BehaviorMessage>> DeleteBehaviorMessage(int id)
        {
            var ev = await _context.Messages.FindAsync(id);
            if (ev == null)
            {
                return NotFound();
            }

            _context.Messages.Remove(ev);
            await _context.SaveChangesAsync();

            return ev;
        }
        [HttpGet("{id}/export")]
        public async Task<ActionResult> ExportMessage(int id)
        {
            var model = await _context.Messages.Include(x => x.Data).FirstOrDefaultAsync(x => x.Id == id);
            if (model == null) return NotFound();

            var json = JsonSerializer.Serialize(model);
            var bytes = Encoding.UTF8.GetBytes(json);

            return File(bytes, "application/json", fileDownloadName: $"{model.Name}.json");
        }


        private bool BehaviorMessageExists(int id)
        {
            return _context.Messages.Any(e => e.Id == id);
        }
    }
}
