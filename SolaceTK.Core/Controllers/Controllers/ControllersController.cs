using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolaceTK.Core.Contexts;
using SolaceTK.Core.Models;
using SolaceTK.Core.Models.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SolaceTK.Core.Models.Core;
using SolaceTK.Core.Models.Sound;

namespace SolaceTK.Core.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class ControllersController : ControllerBase
    {
        private readonly CoreContext _context;
        private readonly BehaviorContext _behaviorContext;
        private readonly SolaceTK.Core.Contexts.ControllerContext _controllerContext;
        private readonly SoundContext _soundContext;

        public ControllersController(CoreContext context, BehaviorContext behaviorContext, SoundContext soundContext, SolaceTK.Core.Contexts.ControllerContext controllerContext)
        {
            _context = context;
            _behaviorContext = behaviorContext;
            _controllerContext = controllerContext;
            _soundContext = soundContext;
        }

        // GET: api/Controller
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovableController>>> GetControllers()
        {
            return await _controllerContext.Controllers.Include(x => x.Components).ThenInclude(x => x.ComponentData)
            .Include(x => x.SoundSet).ThenInclude(x => x.Sources).ThenInclude(x => x.SoundData).ToListAsync();
        }

        // GET: api/Controller/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovableController>> GetController(int id)
        {
            var model = await _controllerContext.Controllers.Include(x => x.Components).ThenInclude(x => x.ComponentData)
            .Include(x => x.SoundSet).ThenInclude(x => x.Sources).ThenInclude(x => x.SoundData).FirstOrDefaultAsync(x => x.Id == id);

            if (model == null)
            {
                return NotFound();
            }

            return model;
        }

        // POST: api/Controller
        [HttpPost]
        public async Task<ActionResult<MovableController>> PostController(MovableController model)
        {
            _controllerContext.Controllers.Add(model);
            var savedCount = await _context.SaveChangesAsync();
            Console.WriteLine($"Core Saved {savedCount} Entities...");
            savedCount = await _behaviorContext.SaveChangesAsync();
            Console.WriteLine($"Behaviors Saved {savedCount} Entities...");
            savedCount = await _controllerContext.SaveChangesAsync();
            Console.WriteLine($"Controllers Saved {savedCount} Entities...");

            return CreatedAtAction("GetController", new { id = model.Id }, model);
        }

        // PUT: api/Controller/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutController(int id, MovableController model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            var entity = (await GetController(id)).Value;

            _controllerContext.Entry(entity).CurrentValues.SetValues(model);

            entity.Components = CheckComponents(entity.Components, model.Components);
            entity.ControllerData = CheckDownstreamData(entity.ControllerData, model.ControllerData);

            if (entity.SoundSet != null)
            {
                _soundContext.Entry(entity.SoundSet).CurrentValues.SetValues(model.SoundSet);
                entity.SoundSet.Sources = CheckSoundSources(entity.SoundSet.Sources, model.SoundSet.Sources);
            }
            else if (model.SoundSet != null) entity.SoundSet.Id = model.SoundSet.Id;
            entity.BehaviorSystemId = model.BehaviorSystemId;

            try
            {
                var savedCount = await _context.SaveChangesAsync();
                Console.WriteLine($"Core Saved {savedCount} Entities...");
                savedCount = await _behaviorContext.SaveChangesAsync();
                Console.WriteLine($"Behaviors Saved {savedCount} Entities...");
                savedCount = await _controllerContext.SaveChangesAsync();
                Console.WriteLine($"Controllers Saved {savedCount} Entities...");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ControllerExists(id))
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

        // DELETE: api/Controller/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<MovableController>> DeleteController(int id)
        {
            var model = await _controllerContext.Controllers.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            _controllerContext.Controllers.Remove(model);
            await _controllerContext.SaveChangesAsync();

            return model;
        }

        [HttpGet("{id}/export")]
        public async Task<ActionResult> ExportController(int id)
        {
            var model = await _controllerContext.Controllers.Include(x => x.Components).ThenInclude(x => x.ComponentData)
            .Include(x => x.SoundSet).ThenInclude(x => x.Sources).ThenInclude(x => x.SoundData).FirstOrDefaultAsync(x => x.Id == id);
            if (model == null) return NotFound();

            var json = JsonSerializer.Serialize(model);
            var bytes = Encoding.UTF8.GetBytes(json);

            return File(bytes, "application/json", fileDownloadName: $"{model.Name}.json");
        }

        private bool ControllerExists(int id)
        {
            return _controllerContext.Controllers.Any(e => e.Id == id);
        }

        private async Task<MovableController> FindController(int id)
        {
            return await _controllerContext.Controllers.FirstOrDefaultAsync(x => x.Id == id);
        }

        private ICollection<SoundSource> CheckSoundSources(ICollection<SoundSource> entities, ICollection<SoundSource> model)
        {
            if (model == null || model.Count == 0) return null;

            var temp = entities.ToList() ?? new List<SoundSource>();
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
                    if (m == null) _soundContext.Entry(temp[i]).State = EntityState.Deleted;
                    else
                    {
                        // Set Values and Check Data:
                        _soundContext.Entry(temp[i]).CurrentValues.SetValues(m);
                        temp[i].SoundData = CheckDownstreamData(temp[i].SoundData, m.SoundData);
                    }
                }
                else _soundContext.Add(temp[i]);
            }
            if (removed.Count > 0) temp.RemoveAll(s => removed.Contains(s.Id));
            return temp;
        }

        private ICollection<SolTkComponent> CheckComponents(ICollection<SolTkComponent> entities, ICollection<SolTkComponent> model)
        {
            if (model == null || model.Count == 0) return null;

            var temp = entities.ToList() ?? new List<SolTkComponent>();
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
                    if (m == null) _controllerContext.Entry(temp[i]).State = EntityState.Deleted;
                    else
                    {
                        // Set Values and Check Data:
                        _controllerContext.Entry(temp[i]).CurrentValues.SetValues(m);
                        temp[i].ComponentData = CheckDownstreamData(temp[i].ComponentData, m.ComponentData);
                    }
                }
                else _controllerContext.Components.Add(temp[i]);
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
                    if (m == null) _controllerContext.Entry(temp[i]).State = EntityState.Deleted;
                    else _controllerContext.Entry(temp[i]).CurrentValues.SetValues(m);
                }
                else _controllerContext.ControllerData.Add(temp[i]);
            }
            if (removed.Count > 0) temp.RemoveAll(s => removed.Contains(s.Id));
            return temp;
        }
    }
}
