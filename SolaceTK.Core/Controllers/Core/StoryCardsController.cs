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
    public class StoryCardsController : ControllerBase
    {
        private readonly CoreContext _context;
        private readonly BehaviorContext _behaviorContext;

        public StoryCardsController(CoreContext context, BehaviorContext behaviorContext)
        {
            _context = context;
            _behaviorContext = behaviorContext;
        }

        // GET: api/StoryCard
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StoryCard>>> GetStoryCards()
        {
            return await _context.StoryCards.Include(x => x.Events).ThenInclude(x => x.Conditions)
                .Include(x => x.Events).ThenInclude(x => x.DownstreamData)
                .Include(x => x.Events).ThenInclude(x => x.Messages).ThenInclude(x => x.Data)
                .Include(x => x.Conditions)
                .Include(x => x.DownstreamData)
                .ToListAsync();
        }

        // GET: api/StoryCard/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StoryCard>> GetStoryCard(int id)
        {
            var model = await _context.StoryCards.Include(x => x.Events).ThenInclude(x => x.Conditions)
                .Include(x => x.Events).ThenInclude(x => x.DownstreamData)
                .Include(x => x.Events).ThenInclude(x => x.Messages).ThenInclude(x => x.Data)
                .Include(x => x.Conditions)
                .Include(x => x.DownstreamData).FirstOrDefaultAsync(x => x.Id == id);

            if (model == null)
            {
                return NotFound();
            }

            return model;
        }

        // POST: api/StoryCard
        [HttpPost]
        public async Task<ActionResult<StoryCard>> PostStoryCard(StoryCard model)
        {
            _context.StoryCards.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStoryCard", new { id = model.Id }, model);
        }

        // PUT: api/StoryCard/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStoryCard(int id, StoryCard model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            _context.Entry(model).State = EntityState.Modified;

            CheckConditions(model.Conditions);
            CheckDownstreamData(model.DownstreamData);
            CheckEvents(model.Events);

            try
            {
                Console.WriteLine($"StoryCard Models Saved on Update: {await _context.SaveChangesAsync()}");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoryCardExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(await GetCard(model.Id));
        }

        // DELETE: api/StoryCard/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<StoryCard>> DeleteStoryCard(int id)
        {
            var model = await _context.StoryCards.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            _context.StoryCards.Remove(model);
            await _context.SaveChangesAsync();

            return model;
        }

        [HttpGet("{id}/export")]
        public async Task<ActionResult> ExportStoryCard(int id)
        {
            var model = await _context.StoryCards.Include(x => x.Events).ThenInclude(x => x.Conditions)
                .Include(x => x.Events).ThenInclude(x => x.DownstreamData)
                .Include(x => x.Events).ThenInclude(x => x.Messages).ThenInclude(x => x.Data)
                .Include(x => x.Conditions)
                .Include(x => x.DownstreamData).FirstOrDefaultAsync(x => x.Id == id);
            if (model == null) return NotFound();

            var json = JsonSerializer.Serialize(model);
            var bytes = Encoding.UTF8.GetBytes(json);

            return File(bytes, "application/json", fileDownloadName: $"{model.Name}.json");
        }

        private bool StoryCardExists(int id)
        {
            return _context.StoryCards.Any(e => e.Id == id);
        }

        private async Task<StoryCard> GetCard(int id)
        {
            return await _context.StoryCards.Include(x => x.Events).ThenInclude(x => x.Conditions)
                .Include(x => x.Events).ThenInclude(x => x.DownstreamData)
                .Include(x => x.Events).ThenInclude(x => x.Messages).ThenInclude(x => x.Data)
                .Include(x => x.Conditions)
                .Include(x => x.DownstreamData).FirstOrDefaultAsync(x => x.Id == id);
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
