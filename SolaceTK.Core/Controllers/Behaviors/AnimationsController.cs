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

namespace SolaceTK.Core.Controllers.Behaviors
{

    [Route("api/v1/Behaviors/[controller]")]
    [ApiController]
    public class AnimationsController : ControllerBase
    {
        private readonly BehaviorContext _context;

        public AnimationsController(BehaviorContext context)
        {
            _context = context;
        }

        // GET: api/BehaviorAnimation
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BehaviorAnimation>>> GetBehaviorAnimations()
        {
            return await _context.Animations.Include(x => x.ActFrameData).ThenInclude(x => x.Frames).ThenInclude(x => x.DownstreamData).ToListAsync();
        }

        // GET: api/BehaviorAnimation/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BehaviorAnimation>> GetBehaviorAnimation(int id)
        {
            var model = await _context.Animations.Include(x => x.ActFrameData).ThenInclude(x => x.Frames).ThenInclude(x => x.DownstreamData).FirstOrDefaultAsync(x => x.Id == id);

            if (model == null)
            {
                return NotFound();
            }

            return model;
        }

        // POST: api/BehaviorAnimation
        [HttpPost]
        public async Task<ActionResult<BehaviorAnimation>> PostBehaviorAnimation(BehaviorAnimation model)
        {
            if (model.ActFrameData == null)
            {
                model.ActFrameData = new BehaviorAnimationData
                {
                    Name = $"{model.Name}",
                    Enabled = false,
                };
            }

            _context.Animations.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBehaviorAnimation", new { id = model.Id }, model);
        }

        // PUT: api/BehaviorAnimation/5
        [HttpPut("{id}")]
        public async Task<ActionResult<BehaviorAnimation>> PutBehaviorAnimation(int id, BehaviorAnimation model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            var entity = (await GetBehaviorAnimation(id)).Value;

            _context.Entry(entity).CurrentValues.SetValues(model);
            _context.Entry(entity.ActFrameData).CurrentValues.SetValues(model.ActFrameData);
            entity.ActFrameData.Frames = CheckFrameData(entity.ActFrameData.Frames, model.ActFrameData.Frames);

            try
            {
                var saved = await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BehaviorAnimationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(model);
        }

        // DELETE: api/BehaviorAnimation/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<BehaviorAnimation>> DeleteBehaviorAnimation(int id)
        {
            var model = await _context.Animations.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            _context.Animations.Remove(model);
            await _context.SaveChangesAsync();

            return model;
        }

        [HttpGet("{id}/export")]
        public async Task<ActionResult> ExportAnimation(int id)
        {
            var model = await _context.Animations.Include(x => x.ActFrameData).ThenInclude(x => x.Frames).ThenInclude(x => x.DownstreamData).FirstOrDefaultAsync(x => x.Id == id);
            if (model == null) return NotFound();

            var json = JsonSerializer.Serialize(model);
            var bytes = Encoding.UTF8.GetBytes(json);

            return File(bytes, "application/json", fileDownloadName: $"{model.Name}.json");
        }

        private bool BehaviorAnimationExists(int id)
        {
            return _context.Animations.Any(e => e.Id == id);
        }

        private ICollection<BehaviorAnimationFrame> CheckFrameData(ICollection<BehaviorAnimationFrame> entities, ICollection<BehaviorAnimationFrame> frames)
        {
            if (frames == null || frames.Count == 0) return entities;

            var temp = entities.ToList() ?? new List<BehaviorAnimationFrame>();
            var removed = new List<int>();

            // Merge Lists:
            if (temp.Count != frames.Count)
            {
                var addedData = frames.Where(x => !temp.Any(t => t.Id == x.Id));
                temp.AddRange(addedData);
            }

            for (int i = 0; i < temp.Count; i++)
            {
                temp[i].Order = i;
                if (temp[i].Id > 0)
                {
                    var m = frames.FirstOrDefault(e => e.Id == temp[i].Id);
                    if (m == null)
                    {
                        _context.Entry(temp[i]).State = EntityState.Deleted;
                        removed.Add(temp[i].Id);
                    }
                    else
                    {
                        _context.Entry(temp[i]).CurrentValues.SetValues(m);
                        temp[i].DownstreamData = CheckDownstreamData(temp[i].DownstreamData, m.DownstreamData);
                    }
                }
                else _context.Frames.Add(temp[i]);
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
    }
}
