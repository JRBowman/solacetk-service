using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolaceTK.Core.Contexts;
using SolaceTK.Core.Models;
using SolaceTK.Core.Models.Behavior;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SolaceTK.Core.Controllers.Events
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly BehaviorContext _context;

        public EventsController(BehaviorContext context)
        {
            _context = context;
        }

        // GET: api/BehaviorEvent
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BehaviorEvent>>> GetBehaviorEvents()
        {
            return await _context.Events.Include("Conditions").Include("DownstreamData")
                 .Include(x => x.Messages).ThenInclude(x => x.Data).ToListAsync();
        }

        // GET: api/BehaviorEvent/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BehaviorEvent>> GetBehaviorEvent(int id)
        {
            var ev = await _context.Events.Include("Conditions").Include("DownstreamData")
                 .Include(x => x.Messages).ThenInclude(x => x.Data).FirstOrDefaultAsync(x => x.Id == id);

            if (ev == null)
            {
                return NotFound();
            }

            return ev;
        }

        // POST: api/BehaviorEvent
        [HttpPost]
        public async Task<ActionResult<BehaviorEvent>> PostBehaviorEvent(BehaviorEvent ev)
        {
            _context.Events.Add(ev);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBehaviorEvent", new { id = ev.Id }, ev);
        }

        // PUT: api/BehaviorEvent/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBehaviorEvent(int id, BehaviorEvent ev)
        {
            if (id != ev.Id)
            {
                return BadRequest();
            }

            var entity = (await GetBehaviorEvent(id)).Value;

            _context.Entry(entity).CurrentValues.SetValues(ev);

            ev.Conditions = CheckConditions(entity.Conditions, ev.Conditions);
            ev.DownstreamData = CheckDownstreamData(entity.DownstreamData, ev.DownstreamData);
            ev.Messages = CheckMessages(entity.Messages, ev.Messages);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BehaviorEventExists(id))
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

        // DELETE: api/BehaviorEvent/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<BehaviorEvent>> DeleteBehaviorEvent(int id)
        {
            var ev = await _context.Events.FindAsync(id);
            if (ev == null)
            {
                return NotFound();
            }

            _context.Events.Remove(ev);
            await _context.SaveChangesAsync();

            return ev;
        }

        [HttpGet("{id}/export")]
        public async Task<ActionResult> ExportEvent(int id)
        {
            var model = await _context.Events.Include("Conditions").Include("DownstreamData")
                 .Include(x => x.Messages).ThenInclude(x => x.Data).FirstOrDefaultAsync(x => x.Id == id);
            if (model == null) return NotFound();

            var json = JsonSerializer.Serialize(model);
            var bytes = Encoding.UTF8.GetBytes(json);

            return File(bytes, "application/json", fileDownloadName: $"{model.Name}.json");
        }

        private bool BehaviorEventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }

        private ICollection<BehaviorMessage> CheckMessages(ICollection<BehaviorMessage> entities, ICollection<BehaviorMessage> model)
        {
            if (model == null || model.Count == 0) return null;

            var temp = entities.ToList() ?? new List<BehaviorMessage>();
            var removed = new List<int>();

            // Merge Lists:
            if (temp.Count != model.Count)
            {
                var addedEvents = model.Where(x => x.Id == 0 || !temp.Any(t => t.Id == x.Id));
                temp.AddRange(addedEvents);
            }

            for (var i = 0; i < temp.Count; i++)
            {
                var m = model.FirstOrDefault(e => e.Id == temp[i].Id);
                if (m == null) removed.Add(temp[i].Id);
                else if (temp[i].Id > 0)
                {
                    _context.Entry(temp[i]).CurrentValues.SetValues(m);
                    temp[i].Data = CheckDownstreamData(temp[i].Data, m.Data);
                }
                else _context.Messages.Add(m);
            }
            if (removed.Count > 0) temp.RemoveAll(s => removed.Contains(s.Id));
            return temp;
        }

        private ICollection<SolTkData> CheckDownstreamData(ICollection<SolTkData> entities, ICollection<SolTkData> model)
        {
            if (model == null || model.Count == 0) return null;

            var temp = entities.ToList() ?? new List<SolTkData>();
            var removed = new List<int>();

            // Merge Lists:
            if (temp.Count != model.Count)
            {
                var addedData = model.Where(x => x.Id == 0 || !temp.Any(t => t.Id == x.Id));
                temp.AddRange(addedData);
            }

            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i].Id > 0)
                {
                    // Updates the underlying Entity to the Model - or Null if the ID isn't found (Removing / Breaking relationship);
                    var m = model.FirstOrDefault(e => e.Id == temp[i].Id);
                    if (m == null)
                    {
                        _context.Entry(temp[i]).State = EntityState.Deleted;
                        removed.Add(temp[i].Id);
                    }
                    else _context.Entry(temp[i]).CurrentValues.SetValues(m);
                }
                else _context.AttributeData.Add(temp[i]);
            }
            if (removed.Count > 0) temp.RemoveAll(s => removed.Contains(s.Id));
            return temp;
        }

        private ICollection<SolTkCondition> CheckConditions(ICollection<SolTkCondition> entities, ICollection<SolTkCondition> model)
        {
            if (model == null || model.Count == 0) return null;

            var temp = entities.ToList() ?? new List<SolTkCondition>();
            var removed = new List<int>();

            // Merge Lists:
            if (temp.Count != model.Count)
            {
                var addedData = model.Where(x => x.Id == 0 || !temp.Any(t => t.Id == x.Id));
                temp.AddRange(addedData);
            }

            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i].Id > 0)
                {
                    // Updates the underlying Entity to the Model - or Null if the ID isn't found (Removing / Breaking relationship);
                    var m = model.FirstOrDefault(e => e.Id == temp[i].Id);
                    if (m == null)
                    {
                        _context.Entry(temp[i]).State = EntityState.Deleted;
                        removed.Add(temp[i].Id);
                    }
                    else _context.Entry(temp[i]).CurrentValues.SetValues(m);
                }
                else _context.ConditionsData.Add(temp[i]);
            }
            if (removed.Count > 0) temp.RemoveAll(s => removed.Contains(s.Id));
            return temp;
        }


    }
}
