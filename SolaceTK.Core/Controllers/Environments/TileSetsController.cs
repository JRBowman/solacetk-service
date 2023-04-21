using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolaceTK.Core.Contexts;
using SolaceTK.Core.Models;
using SolaceTK.Core.Models.Environment;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SolaceTK.Core.Controllers.Environments
{

    [Route("api/v1/Environments/[controller]")]
    [ApiController]
    public class TileSetsController : ControllerBase
    {
        private readonly EnvironmentContext _context;

        public TileSetsController(EnvironmentContext context)
        {
            _context = context;
        }

        // GET: api/TileSet
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TileSet>>> GetTileSets()
        {
            return await _context.TileSets.Include(x => x.Tiles).ThenInclude(x => x.Rules)
                .Include(x => x.Tiles).ThenInclude(x => x.Data)
                .ToListAsync();
        }

        // GET: api/TileSet/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TileSet>> GetTileSet(int id)
        {
            var ev = await _context.TileSets.Include(x => x.Tiles).ThenInclude(x => x.Rules)
                .Include(x => x.Tiles).ThenInclude(x => x.Data).FirstOrDefaultAsync(x => x.Id == id);

            if (ev == null)
            {
                return NotFound();
            }

            return ev;
        }

        // POST: api/TileSet
        [HttpPost]
        public async Task<ActionResult<TileSet>> PostTileSet(TileSet ev)
        {
            _context.TileSets.Add(ev);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTileSet", new { id = ev.Id }, ev);
        }

        // PUT: api/TileSet/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTileSet(int id, TileSet ev)
        {
            if (id != ev.Id)
            {
                return BadRequest();
            }

            // Get Exsisting Entity and Update Values:
            var entity = (await GetTileSet(id)).Value;

            _context.Entry(entity).CurrentValues.SetValues(ev);

            entity.Tiles = CheckTiles(entity.Tiles, ev.Tiles);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TileSetExists(id))
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

        // DELETE: api/TileSet/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TileSet>> DeleteTileSet(int id)
        {
            var ev = await _context.TileSets.FindAsync(id);
            if (ev == null)
            {
                return NotFound();
            }

            _context.TileSets.Remove(ev);
            await _context.SaveChangesAsync();

            return ev;
        }

        [HttpGet("{id}/export")]
        public async Task<ActionResult> ExportTileSet(int id)
        {
            var model = await _context.TileSets.Include(x => x.Tiles).ThenInclude(x => x.Rules)
                .Include(x => x.Tiles).ThenInclude(x => x.Data).FirstOrDefaultAsync(x => x.Id == id);
            if (model == null) return NotFound();

            var json = JsonSerializer.Serialize(model);
            var bytes = Encoding.UTF8.GetBytes(json);

            return File(bytes, "application/json", fileDownloadName: $"{model.Name}.json");
        }

        private bool TileSetExists(int id)
        {
            return _context.TileSets.Any(e => e.Id == id);
        }


        private ICollection<Tile> CheckTiles(ICollection<Tile> entities, ICollection<Tile> model)
        {
            if (model == null || model.Count == 0) return null;

            var temp = entities != null ? entities.ToList() : new List<Tile>();

            // Merge Lists:
            if (temp.Count != model.Count)
            {
                var addedData = model.Where(x => !temp.Any(t => t.Id == x.Id) || x.Id == 0);
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
                    else
                    {
                        _context.Entry(temp[i]).CurrentValues.SetValues(m);
                        temp[i].Rules = CheckRules(temp[i].Rules, m.Rules);
                        temp[i].Data = CheckDownstreamData(temp[i].Data, m.Data);
                    }


                }
                else _context.Tiles.Add(temp[i]);
            }

            return temp;
        }

        private ICollection<TileRule> CheckRules(ICollection<TileRule> entities, ICollection<TileRule> model)
        {
            if (model == null || model.Count == 0) return null;

            var temp = entities != null ? entities.ToList() : new List<TileRule>();

            // Merge Lists:
            if (temp.Count != model.Count)
            {
                var addedData = model.Where(x => !temp.Any(t => t.Id == x.Id) || x.Id == 0);
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
                else _context.TileRules.Add(temp[i]);
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
                else _context.EnvironmentData.Add(temp[i]);
            }

            return temp;
        }
    }
}
