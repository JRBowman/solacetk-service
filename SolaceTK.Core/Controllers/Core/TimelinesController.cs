using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolaceTK.Core.Contexts;
using SolaceTK.Core.Models;
using SolaceTK.Core.Models.Behavior;
using SolaceTK.Core.Models.Core;
using SolaceTK.Core.Models.Story;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SolaceTK.Core.Controllers.Core
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class TimelinesController : ControllerBase
    {
        private readonly CoreContext _context;
        private readonly BehaviorContext _behaviorContext;

        public TimelinesController(CoreContext context, BehaviorContext behaviorContext)
        {
            _context = context;
            _behaviorContext = behaviorContext;
        }

        // GET: api/Timeline
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Timeline>>> GetTimelines()
        {
            return await _context.Timelines.Include(x => x.StoryCards).ThenInclude(x => x.Events).ThenInclude(x => x.Conditions)
                .Include(x => x.StoryCards).ThenInclude(x => x.Events).ThenInclude(x => x.DownstreamData)
                .Include(x => x.StoryCards).ThenInclude(x => x.Events).ThenInclude(x => x.Messages).ThenInclude(x => x.Data)
                .Include(x => x.StoryCards).ThenInclude(x => x.Conditions)
                .Include(x => x.StoryCards).ThenInclude(x => x.DownstreamData)
                .ToListAsync();
        }

        // GET: api/Timeline/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Timeline>> GetTimeline(int id)
        {
            var model = await _context.Timelines.Include(x => x.StoryCards).ThenInclude(x => x.Events).ThenInclude(x => x.Conditions)
                .Include(x => x.StoryCards).ThenInclude(x => x.Events).ThenInclude(x => x.DownstreamData)
                .Include(x => x.StoryCards).ThenInclude(x => x.Events).ThenInclude(x => x.Messages).ThenInclude(x => x.Data)
                .Include(x => x.StoryCards).ThenInclude(x => x.Conditions)
                .Include(x => x.StoryCards).ThenInclude(x => x.DownstreamData).FirstOrDefaultAsync(x => x.Id == id);

            if (model == null)
            {
                return NotFound();
            }

            return model;
        }

        // POST: api/Timeline
        [HttpPost]
        public async Task<ActionResult<Timeline>> PostTimeline(Timeline model)
        {
            _context.Timelines.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTimeline", new { id = model.Id }, model);
        }

        // PUT: api/Timeline/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTimeline(int id, Timeline model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            _context.Entry(model).State = EntityState.Modified;
            CheckStoryCards(model.StoryCards);

            try
            {
                Console.WriteLine($"Timeline Models Saved on Update: {await _context.SaveChangesAsync()}");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TimelineExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(await GetTimelineById(model.Id));
        }

        // DELETE: api/Timeline/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Timeline>> DeleteTimeline(int id)
        {
            var model = await _context.Timelines.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            _context.Timelines.Remove(model);
            await _context.SaveChangesAsync();

            return model;
        }

        [HttpGet("{id}/export")]
        public async Task<ActionResult> ExportTimeline(int id)
        {
            var model = await _context.Timelines.Include(x => x.StoryCards).ThenInclude(x => x.Events).ThenInclude(x => x.Conditions)
                .Include(x => x.StoryCards).ThenInclude(x => x.Events).ThenInclude(x => x.DownstreamData)
                .Include(x => x.StoryCards).ThenInclude(x => x.Events).ThenInclude(x => x.Messages).ThenInclude(x => x.Data)
                .Include(x => x.StoryCards).ThenInclude(x => x.Conditions)
                .Include(x => x.StoryCards).ThenInclude(x => x.DownstreamData).FirstOrDefaultAsync(x => x.Id == id);
            if (model == null) return NotFound();

            var json = JsonSerializer.Serialize(model);
            var bytes = Encoding.UTF8.GetBytes(json);

            return File(bytes, "application/json", fileDownloadName: $"{model.Name}.json");
        }

        private bool TimelineExists(int id)
        {
            return _context.Timelines.Any(e => e.Id == id);
        }

        private async Task<Timeline> GetTimelineById(int id)
        {
            return await _context.Timelines.Include(x => x.StoryCards).ThenInclude(x => x.Events).ThenInclude(x => x.Conditions)
                .Include(x => x.StoryCards).ThenInclude(x => x.Events).ThenInclude(x => x.DownstreamData)
                .Include(x => x.StoryCards).ThenInclude(x => x.Events).ThenInclude(x => x.Messages).ThenInclude(x => x.Data)
                .Include(x => x.StoryCards).ThenInclude(x => x.Conditions)
                .Include(x => x.StoryCards).ThenInclude(x => x.DownstreamData).FirstOrDefaultAsync(x => x.Id == id);
        }

        private void CheckStoryCards(ICollection<StoryCard> cards)
        {
            if (cards == null || cards.Count == 0) return;
            foreach (var card in cards) 
            {
                if (card.Id > 0) _context.Entry(card).State = EntityState.Modified;
                else _context.StoryCards.Add(card);

                CheckConditions(card.Conditions);
                CheckDownstreamData(card.DownstreamData);
                CheckEvents(card.Events);
            }
        }

        private void CheckEvents(ICollection<BehaviorEvent> events)
        {
            if (events == null || events.Count == 0) return;
            foreach (var ev in events)
            {
                if (ev.Id > 0) _context.Entry(ev).State = EntityState.Modified;
                else _behaviorContext.Events.Add(ev);

                CheckConditions(ev.Conditions);
                CheckDownstreamData(ev.DownstreamData);
                CheckMessages(ev.Messages);
            }
        }

        private void CheckMessages(ICollection<BehaviorMessage> model)
        {
            if (model == null) return;
            foreach (var m in model)
            {
                if (m.Id > 0) _context.Entry(m).State = EntityState.Modified;
                else _behaviorContext.Messages.Add(m);

                CheckDownstreamData(m.Data);
            }
        }

        private void CheckDownstreamData(ICollection<SolTkData> model)
        {
            if (model == null) return;
            foreach (var m in model)
            {
                if (m.Id > 0) _context.Entry(m).State = EntityState.Modified;
                else _behaviorContext.AttributeData.Add(m);
            }
        }

        private void CheckConditions(ICollection<SolTkCondition> model)
        {
            if (model == null) return;
            foreach (var m in model)
            {
                if (m.Id > 0) _context.Entry(m).State = EntityState.Modified;
                else _behaviorContext.ConditionsData.Add(m);
            }
        }
    }
}
