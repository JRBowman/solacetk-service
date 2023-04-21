using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using SolaceTK.Core.Contexts;
using SolaceTK.Core.Models;
using SolaceTK.Core.Models.Behavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SolaceTK.Core.Controllers
{

    [Route("api/v1/Behaviors/[controller]")]
    [ApiController]
    public class SystemsController : ControllerBase
    {
        private readonly BehaviorContext _context;

        public SystemsController(BehaviorContext context)
        {
            _context = context;
        }

        // GET: api/BehaviorSystem
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BehaviorSystem>>> GetBehaviorSystems()
        {
            return await _context.Systems.Include(x => x.VarData)
                .Include(x => x.Events).ThenInclude(x => x.Conditions)
                .Include(x => x.Events).ThenInclude(x => x.DownstreamData)
                .Include(x => x.Events).ThenInclude(x => x.Messages).ThenInclude(x => x.Data)
                .Include(x => x.Behaviors).ThenInclude(x => x.Events).ThenInclude(x => x.Conditions)
                .Include(x => x.Behaviors).ThenInclude(x => x.Events).ThenInclude(x => x.DownstreamData)
                .Include(x => x.Behaviors).ThenInclude(x => x.Events).ThenInclude(x => x.Messages).ThenInclude(x => x.Data)
                .Include(x => x.Behaviors).ThenInclude(x => x.Animation).ThenInclude(x => x.ActFrameData).ThenInclude(x => x.Frames).ThenInclude(x => x.DownstreamData)
                .Include(x => x.Behaviors).ThenInclude(x => x.StartData)
                .Include(x => x.Behaviors).ThenInclude(x => x.EndData)
                .Include(x => x.Behaviors).ThenInclude(x => x.Conditions)
                .Include(x => x.Behaviors).ThenInclude(x => x.NextStates)
                .Include(x => x.Behaviors).ThenInclude(x => x.ActData).ToListAsync();
        }

        // GET: api/BehaviorSystem/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BehaviorSystem>> GetBehaviorSystem(int id)
        {
            var behaviorSystem = await _context.Systems.Include(x => x.VarData)
                .Include(x => x.Events).ThenInclude(x => x.Conditions)
                .Include(x => x.Events).ThenInclude(x => x.DownstreamData)
                .Include(x => x.Events).ThenInclude(x => x.Messages).ThenInclude(x => x.Data)
                .Include(x => x.Behaviors).ThenInclude(x => x.Events).ThenInclude(x => x.Conditions)
                .Include(x => x.Behaviors).ThenInclude(x => x.Events).ThenInclude(x => x.DownstreamData)
                .Include(x => x.Behaviors).ThenInclude(x => x.Events).ThenInclude(x => x.Messages).ThenInclude(x => x.Data)
                .Include(x => x.Behaviors).ThenInclude(x => x.Animation).ThenInclude(x => x.ActFrameData).ThenInclude(x => x.Frames).ThenInclude(x => x.DownstreamData)
                .Include(x => x.Behaviors).ThenInclude(x => x.StartData)
                .Include(x => x.Behaviors).ThenInclude(x => x.EndData)
                .Include(x => x.Behaviors).ThenInclude(x => x.Conditions)
                .Include(x => x.Behaviors).ThenInclude(x => x.NextStates)
                .Include(x => x.Behaviors).ThenInclude(x => x.ActData).FirstOrDefaultAsync(x => x.Id == id);

            if (behaviorSystem == null)
            {
                return NotFound();
            }

            return behaviorSystem;
        }

        // POST: api/BehaviorSystem
        [HttpPost]
        public async Task<ActionResult<BehaviorSystem>> PostBehaviorSystem(BehaviorSystem behaviorSystem)
        {
            _context.Systems.Add(behaviorSystem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBehaviorSystem", new { id = behaviorSystem.Id }, behaviorSystem);
        }

        // PUT: api/BehaviorSystem/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBehaviorSystem(int id, BehaviorSystem behaviorSystem)
        {
            if (id != behaviorSystem.Id)
            {
                return BadRequest();
            }

            var bs = (await GetBehaviorSystem(id)).Value;

            _context.Entry(bs).CurrentValues.SetValues(behaviorSystem);
            bs.Behaviors = CheckBehaviors(bs.Behaviors, behaviorSystem.Behaviors);
            bs.VarData = CheckDownstreamData(bs.VarData, behaviorSystem.VarData);
            bs.Events = CheckEvents(bs.Events, behaviorSystem.Events);

            try
            {
                Console.WriteLine($"Changes Saved: {await _context.SaveChangesAsync()}");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BehaviorSystemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(behaviorSystem);
        }

        // DELETE: api/BehaviorSystem/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<BehaviorSystem>> DeleteBehaviorSystem(int id)
        {
            var behaviorSystem = await _context.Systems.FindAsync(id);
            if (behaviorSystem == null)
            {
                return NotFound();
            }

            _context.Systems.Remove(behaviorSystem);
            await _context.SaveChangesAsync();

            return behaviorSystem;
        }

        [HttpGet("{id}/export")]
        public async Task<ActionResult> ExportSystem(int id)
        {
            var model = await _context.Systems.Include(x => x.VarData)
                .Include(x => x.Events).ThenInclude(x => x.Conditions)
                .Include(x => x.Events).ThenInclude(x => x.DownstreamData)
                .Include(x => x.Events).ThenInclude(x => x.Messages).ThenInclude(x => x.Data)
                .Include(x => x.Behaviors).ThenInclude(x => x.Events).ThenInclude(x => x.Conditions)
                .Include(x => x.Behaviors).ThenInclude(x => x.Events).ThenInclude(x => x.DownstreamData)
                .Include(x => x.Behaviors).ThenInclude(x => x.Events).ThenInclude(x => x.Messages).ThenInclude(x => x.Data)
                .Include(x => x.Behaviors).ThenInclude(x => x.Animation).ThenInclude(x => x.ActFrameData).ThenInclude(x => x.Frames).ThenInclude(x => x.DownstreamData)
                .Include(x => x.Behaviors).ThenInclude(x => x.StartData)
                .Include(x => x.Behaviors).ThenInclude(x => x.EndData)
                .Include(x => x.Behaviors).ThenInclude(x => x.Conditions)
                .Include(x => x.Behaviors).ThenInclude(x => x.NextStates)
                .Include(x => x.Behaviors).ThenInclude(x => x.ActData).FirstOrDefaultAsync(x => x.Id == id);
            if (model == null) return NotFound();

            var json = JsonSerializer.Serialize(model);
            var bytes = Encoding.UTF8.GetBytes(json);

            return File(bytes, "application/json", fileDownloadName: $"{model.Name}.json");
        }

        private bool BehaviorSystemExists(int id)
        {
            return _context.Systems.Any(e => e.Id == id);
        }

        private ICollection<BehaviorState> CheckBehaviors(ICollection<BehaviorState> entity, ICollection<BehaviorState> model)
        {
            if (model == null || model.Count == 0) return null;

            var temp = entity.ToList();
            if (temp.Count != model.Count)
            {
                var addedBehaviors = model.Where(x => !temp.Any(t => t.Id == x.Id));
                temp.AddRange(addedBehaviors);
            }

            for (int i = 0; i < temp.Count; i++)
            {
                var m = model.FirstOrDefault(e => e.Id == temp[i].Id);
                if (m == null) temp.RemoveAt(i);
                else _context.Entry(temp[i]).CurrentValues.SetValues(m);
            }

            return temp;
        }

        private ICollection<BehaviorEvent> CheckEvents(ICollection<BehaviorEvent> entities, ICollection<BehaviorEvent> model)
        {
            if (model == null || model.Count == 0) return null;

            var temp = entities.ToList();

            // Merge Lists:
            if (temp.Count != model.Count)
            {
                var addedEvents = model.Where(x => !temp.Any(t => t.Id == x.Id));
                temp.AddRange(addedEvents);
            }

            // Existing:
            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i].Id > 0)
                {
                    var m = model.FirstOrDefault(e => e.Id == temp[i].Id);
                    if (m == null) temp.RemoveAt(i);
                    else
                    {
                        _context.Entry(temp[i]).CurrentValues.SetValues(m);

                        temp[i].Messages = CheckMessages(temp[i].Messages, m.Messages);
                        temp[i].DownstreamData = CheckDownstreamData(temp[i].DownstreamData, m.DownstreamData);
                        temp[i].Conditions = CheckConditions(temp[i].Conditions, m.Conditions);
                    }
                }
                else _context.Events.Add(temp[i]);
            }

            return temp;
        }

        private ICollection<BehaviorMessage> CheckMessages(ICollection<BehaviorMessage> entities, ICollection<BehaviorMessage> model)
        {
            if (model == null || model.Count == 0) return null;

            var temp = entities.ToList();

            // Merge Lists:
            if (temp.Count != model.Count)
            {
                var addedEvents = model.Where(x => !temp.Any(t => t.Id == x.Id));
                temp.AddRange(addedEvents);
            }

            for (var i = 0; i < temp.Count; i++)
            {
                var m = model.FirstOrDefault(e => e.Id == temp[i].Id);
                if (m == null) temp.RemoveAt(i);
                else if (temp[i].Id > 0)
                {
                    _context.Entry(temp[i]).CurrentValues.SetValues(m);
                    temp[i].Data = CheckDownstreamData(temp[i].Data, m.Data);
                }
                else _context.Messages.Add(m);
            }

            return temp;
        }

        private ICollection<SolTkData> CheckDownstreamData(ICollection<SolTkData> entities, ICollection<SolTkData> model)
        {
            if (model == null || model.Count == 0) return null;

            var temp = entities.ToList();

            // Merge Lists:
            if (temp.Count != model.Count)
            {
                var addedData = model.Where(x => !temp.Any(t => t.Id == x.Id));
                temp.AddRange(addedData);
            }

            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i].Id > 0)
                {
                    // Updates the underlying Entity to the Model - or Null if the ID isn't found (Removing / Breaking relationship);
                    var m = model.FirstOrDefault(e => e.Id == temp[i].Id);
                    // Remove Model - No longer in list:
                    if (m == null) 
                    {
                        _context.Entry(temp[i]).State = EntityState.Deleted;
                        temp.RemoveAt(i);
                    }
                    else _context.Entry(temp[i]).CurrentValues.SetValues(m);
                }
                // Model is new to the entity (ID of 0):
                else _context.AttributeData.Add(temp[i]);
            }

            return temp;
        }

        private ICollection<SolTkCondition> CheckConditions(ICollection<SolTkCondition> entities, ICollection<SolTkCondition> model)
        {
            if (model == null || model.Count == 0) return null;

            var temp = entities.ToList();

            // Merge Lists:
            if (temp.Count != model.Count)
            {
                var addedData = model.Where(x => !temp.Any(t => t.Id == x.Id));
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
                        temp.RemoveAt(i);
                    }
                    else _context.Entry(temp[i]).CurrentValues.SetValues(m);
                }
                else _context.ConditionsData.Add(temp[i]);
            }

            return temp;
        }
    }
}
