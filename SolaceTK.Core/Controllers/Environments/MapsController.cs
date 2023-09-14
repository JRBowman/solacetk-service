using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolaceTK.Core.Contexts;
using SolaceTK.Core.Models;
using SolaceTK.Core.Models.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SolaceTK.Core.Controllers.Environments
{

    [Route("api/v1/Environments/[controller]")]
    [ApiController]
    public class MapsController : ControllerBase
    {
        private readonly EnvironmentContext _context;
        private readonly CoreContext _coreContext;

        public MapsController(EnvironmentContext context)
        {
            _context = context;
        }

        // GET: api/Map
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Map>>> GetMaps()
        {
            return await _context.Maps.Include(x => x.Layers).ThenInclude(x => x.LayerData)
                    .Include(x => x.TileSet)
                   .Include(x => x.Cells).ThenInclude(x => x.ActiveData)
                   .Include(x => x.Cells).ThenInclude(x => x.BehaviorEvents).ThenInclude(x => x.Conditions)
                   .Include(x => x.Cells).ThenInclude(x => x.BehaviorEvents).ThenInclude(x => x.DownstreamData)
                   .Include(x => x.Cells).ThenInclude(x => x.BehaviorEvents).ThenInclude(x => x.Messages)
                   .Include(x => x.Cells).ThenInclude(x => x.BehaviorEvents).ThenInclude(x => x.Messages).ThenInclude(x => x.Data)
                   .Include(x => x.Chunks).ThenInclude(x => x.Cells).ToListAsync();
        }

        // GET: api/Map/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Map>> GetMap(int id)
        {
            var ev = await _context.Maps.Include(x => x.Layers).ThenInclude(x => x.LayerData)
                    .Include(x => x.TileSet)
                   .Include(x => x.Cells).ThenInclude(x => x.ActiveData)
                   .Include(x => x.Cells).ThenInclude(x => x.BehaviorEvents).ThenInclude(x => x.Conditions)
                   .Include(x => x.Cells).ThenInclude(x => x.BehaviorEvents).ThenInclude(x => x.DownstreamData)
                   .Include(x => x.Cells).ThenInclude(x => x.BehaviorEvents).ThenInclude(x => x.Messages)
                   .Include(x => x.Cells).ThenInclude(x => x.BehaviorEvents).ThenInclude(x => x.Messages).ThenInclude(x => x.Data)
                   .Include(x => x.Chunks).ThenInclude(x => x.Cells).FirstOrDefaultAsync(x => x.Id == id);

            if (ev == null)
            {
                return NotFound();
            }

            return ev;
        }

        // POST: api/Map
        [HttpPost]
        public async Task<ActionResult<Map>> PostMap(Map ev)
        {
            _context.Maps.Add(ev);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMap", new { id = ev.Id }, ev);
        }

        // PUT: api/Map/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMap(int id, Map map)
        {
            if (id != map.Id)
            {
                return BadRequest();
            }

            var entity = (await GetMap(id)).Value;

            _context.Entry(entity).CurrentValues.SetValues(map);

            entity.Layers = CheckLayers(entity.Layers, map.Layers);

            entity.Cells = CheckCells(entity.Cells, map.Cells);

            try
            {
                var savedCount = await _context.SaveChangesAsync();
                Console.WriteLine($"Saved {savedCount} Entities...");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MapExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(map);
        }

        // DELETE: api/Map/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Map>> DeleteMap(int id)
        {
            var ev = await _context.Maps.FindAsync(id);
            if (ev == null)
            {
                return NotFound();
            }

            _context.Maps.Remove(ev);
            await _context.SaveChangesAsync();

            return ev;
        }

        [HttpGet("{id}/export")]
        public async Task<ActionResult> ExportMap(int id)
        {
            var model = await _context.Maps.Include(x => x.Layers).ThenInclude(x => x.LayerData)
                    .Include(x => x.TileSet)
                   .Include(x => x.Cells).ThenInclude(x => x.ActiveData)
                   .Include(x => x.Cells).ThenInclude(x => x.BehaviorEvents).ThenInclude(x => x.Conditions)
                   .Include(x => x.Cells).ThenInclude(x => x.BehaviorEvents).ThenInclude(x => x.DownstreamData)
                   .Include(x => x.Cells).ThenInclude(x => x.BehaviorEvents).ThenInclude(x => x.Messages)
                   .Include(x => x.Cells).ThenInclude(x => x.BehaviorEvents).ThenInclude(x => x.Messages).ThenInclude(x => x.Data)
                   .Include(x => x.Chunks).ThenInclude(x => x.Cells).FirstOrDefaultAsync(x => x.Id == id);
            if (model == null) return NotFound();

            var json = JsonSerializer.Serialize(model);
            var bytes = Encoding.UTF8.GetBytes(json);

            return File(bytes, "application/json", fileDownloadName: $"{model.Name}.json");
        }

        private bool MapExists(int id)
        {
            return _context.Maps.Any(e => e.Id == id);
        }


        private ICollection<MapLayer> CheckLayers(ICollection<MapLayer> entities, ICollection<MapLayer> model)
        {
            if (model == null || model.Count == 0) return null;

            var temp = entities != null ? entities.ToList() : new List<MapLayer>();

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
                    if (m == null) _context.Entry(temp[i]).State = EntityState.Deleted;
                    else
                    {
                        _context.Entry(temp[i]).CurrentValues.SetValues(m);
                        temp[i].LayerData = CheckDownstreamData(temp[i].LayerData, m.LayerData);
                    }


                }
                else _context.Layers.Add(temp[i]);
            }
            return temp;
        }

        private ICollection<MapCell> CheckCells(ICollection<MapCell> entities, ICollection<MapCell> model)
        {
            if (model == null || model.Count == 0) return null;


            var temp = entities != null ? entities.ToList() : new List<MapCell>();
            var removed = new List<int>();

            // Merge Lists:
            if (temp.Count != model.Count)
            {
                var addedData = model.Where(x => (!temp.Any(t => t.Id == x.Id) || x.Id == 0) && x.Enabled);
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
                    else
                    {
                        _context.Entry(temp[i]).CurrentValues.SetValues(m);
                        temp[i].EnterData = CheckDownstreamData(temp[i].EnterData, m.EnterData);
                        temp[i].ActiveData = CheckDownstreamData(temp[i].ActiveData, m.ActiveData);
                        temp[i].ExitData = CheckDownstreamData(temp[i].ExitData, m.ExitData);
                    }
                }
                else _context.Cells.Add(temp[i]);


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
                else _context.EnvironmentData.Add(temp[i]);
            }
            if (removed.Count > 0) temp.RemoveAll(s => removed.Contains(s.Id));
            return temp;
        }
    }
}
