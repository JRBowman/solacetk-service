using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolaceTK.Core.Contexts;
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
    public class LayersController : ControllerBase
    {
        private readonly EnvironmentContext _context;

        public LayersController(EnvironmentContext context)
        {
            _context = context;
        }

        // GET: api/MapLayer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MapLayer>>> GetMapLayers()
        {
            return await _context.Layers.Include(x => x.LayerData).ToListAsync();
        }

        // GET: api/MapLayer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MapLayer>> GetMapLayer(int id)
        {
            var ev = await _context.Layers.FindAsync(id);

            if (ev == null)
            {
                return NotFound();
            }

            return ev;
        }

        // POST: api/MapLayer
        [HttpPost]
        public async Task<ActionResult<MapLayer>> PostMapLayer(MapLayer ev)
        {
            _context.Layers.Add(ev);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMapLayer", new { id = ev.Id }, ev);
        }

        // PUT: api/MapLayer/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMapLayer(int id, MapLayer ev)
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
                if (!MapLayerExists(id))
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

        // DELETE: api/MapLayer/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<MapLayer>> DeleteMapLayer(int id)
        {
            var ev = await _context.Layers.FindAsync(id);
            if (ev == null)
            {
                return NotFound();
            }

            _context.Layers.Remove(ev);
            await _context.SaveChangesAsync();

            return ev;
        }

        [HttpGet("{id}/export")]
        public async Task<ActionResult> ExportMapLayer(int id)
        {
            var model = await _context.Layers.Include(x => x.LayerData).FirstOrDefaultAsync(x => x.Id == id);
            if (model == null) return NotFound();

            var json = JsonSerializer.Serialize(model);
            var bytes = Encoding.UTF8.GetBytes(json);

            return File(bytes, "application/json", fileDownloadName: $"{model.Name}.json");
        }

        private bool MapLayerExists(int id)
        {
            return _context.Layers.Any(e => e.Id == id);
        }
    }
}
