using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolaceTK.Core.Contexts;
using SolaceTK.Core.Models;
using SolaceTK.Core.Models.Behavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SolaceTK.Core.Controllers.Behaviors
{

    [Route("api/v1/Behaviors/[controller]")]
    [ApiController]
    public class StatesController : ControllerBase
    {
        private readonly BehaviorContext _context;

        public StatesController(BehaviorContext context)
        {
            _context = context;
        }

        // GET: api/BehaviorState
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BehaviorState>>> GetBehaviorStates([FromQuery]bool noop = false)
        {
            return !noop 
                ? await _context.States
                .Include(x => x.Animation).ThenInclude(x => x.ActFrameData).ThenInclude(x => x.Frames).ThenInclude(x => x.DownstreamData)
                .Include(x => x.Conditions)
                .Include("NextStates")
                .Include(x => x.Events).ThenInclude(x => x.Messages).ThenInclude(x => x.Data)
                .Include(x => x.Events).ThenInclude(x => x.Conditions)
                .Include(x => x.Events).ThenInclude(x => x.DownstreamData)
                .Include("StartData").Include("EndData").Include("ActData").Where(x => x.NoOp == false).ToListAsync() 
                : await _context.States
                .Include(x => x.Conditions)
                .Include("NextStates")
                .Include(x => x.Events).ThenInclude(x => x.Messages).ThenInclude(x => x.Data)
                .Include(x => x.Events).ThenInclude(x => x.Conditions)
                .Include(x => x.Events).ThenInclude(x => x.DownstreamData)
                .Include("StartData").Include("EndData").Include("ActData").Where(x => x.NoOp == true).ToListAsync();
        }

        // GET: api/BehaviorState/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BehaviorState>> GetBehaviorState(int id, [FromQuery]bool noop = false)
        {
            var state = !noop 
                ? await _context.States
                .Include(x => x.Animation).ThenInclude(x => x.ActFrameData).ThenInclude(x => x.Frames).ThenInclude(x => x.DownstreamData)
                .Include(x => x.Conditions)
                .Include("NextStates")
                .Include(x => x.Events).ThenInclude(x => x.Messages).ThenInclude(x => x.Data)
                .Include(x => x.Events).ThenInclude(x => x.Conditions)
                .Include(x => x.Events).ThenInclude(x => x.DownstreamData)
                .Include("StartData").Include("EndData").Include("ActData").FirstOrDefaultAsync(x => x.Id == id) 
                : await _context.States
                .Include(x => x.Conditions)
                .Include("NextStates")
                .Include(x => x.Events).ThenInclude(x => x.Messages).ThenInclude(x => x.Data)
                .Include(x => x.Events).ThenInclude(x => x.Conditions)
                .Include(x => x.Events).ThenInclude(x => x.DownstreamData)
                .Include("StartData").Include("EndData").Include("ActData").FirstOrDefaultAsync(x => x.Id == id && x.NoOp == true);

            if (state == null)
            {
                return NotFound();
            }

            return state;
        }

        // POST: api/BehaviorState
        [HttpPost]
        public async Task<ActionResult<BehaviorState>> PostBehaviorState(BehaviorState state)
        {
            _context.States.Add(state);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBehaviorState", new { id = state.Id }, state);
        }
        
        // PUT: api/BehaviorState/5
        [HttpPut("{id}")]
        public async Task<ActionResult<BehaviorState>> PutBehaviorState(int id, BehaviorState state)
        {
            if (id != state.Id)
            {
                return BadRequest();
            }

            var entity = (await GetBehaviorState(id, state.NoOp)).Value;

            _context.Entry(entity).CurrentValues.SetValues(state);
            if (entity.Animation != null && entity.Animation.Id == state.Animation.Id) _context.Entry(entity.Animation).CurrentValues.SetValues(state.Animation);
            else if (entity.Animation != null && entity.Animation.Id != state.Animation.Id)
            {
                entity.Animation = state.Animation;
            }
            else entity.Animation = state.Animation;

            entity.Conditions = CheckConditions(entity.Conditions, state.Conditions);
            entity.StartData = CheckDownstreamData(entity.StartData, state.StartData);
            entity.ActData = CheckDownstreamData(entity.ActData, state.ActData);
            entity.EndData = CheckDownstreamData(entity.EndData, state.EndData);
            entity.Events = CheckEvents(entity.Events, state.Events);

            if (state.NoOp) entity.NextStates = CheckNextStates(entity.NextStates, state.NextStates);

            try
            {
                Console.WriteLine($"Changes Saved: {await _context.SaveChangesAsync()}");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BehaviorStateExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(state);
        }

        // DELETE: api/BehaviorState/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<BehaviorState>> DeleteBehaviorState(int id)
        {
            var state = await _context.States.FindAsync(id);
            if (state == null)
            {
                return NotFound();
            }

            _context.States.Remove(state);
            await _context.SaveChangesAsync();

            return state;
        }

        [HttpGet("{id}/export")]
        public async Task<ActionResult> ExportState(int id, [FromQuery]bool noop = false)
        {
            var model = await _context.States
                .Include(x => x.Animation).ThenInclude(x => x.ActFrameData).ThenInclude(x => x.Frames).ThenInclude(x => x.DownstreamData)
                .Include(x => x.Conditions)
                .Include("NextStates")
                .Include(x => x.Events).ThenInclude(x => x.Messages).ThenInclude(x => x.Data)
                .Include(x => x.Events).ThenInclude(x => x.Conditions)
                .Include(x => x.Events).ThenInclude(x => x.DownstreamData)
                .Include("StartData").Include("EndData").Include("ActData").FirstOrDefaultAsync(x => x.Id == id);
            if (model == null) return NotFound();

            var json = JsonSerializer.Serialize(model);
            var bytes = Encoding.UTF8.GetBytes(json);

            return File(bytes, "application/json", fileDownloadName: $"{model.Name}.json");
        }

        private bool BehaviorStateExists(int id)
        {
            return _context.States.Any(e => e.Id == id);
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
                    if (m == null) 
                    {
                        _context.Entry(temp[i]).State = EntityState.Deleted;
                        temp.RemoveAt(i);
                    }
                    else _context.Entry(temp[i]).CurrentValues.SetValues(m);
                }
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

        private ICollection<BehaviorState> CheckNextStates(ICollection<BehaviorState> entities, ICollection<BehaviorState> model)
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
                    if (m == null) temp.RemoveAt(i);
                    else
                    {
                        _context.Entry(temp[i]).CurrentValues.SetValues(m);
                    }
                }
                else _context.States.Add(temp[i]);
            }

            return temp;
        }
    }
}
