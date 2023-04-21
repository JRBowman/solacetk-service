using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolaceTK.Core.Contexts;
using SolaceTK.Core.Models.Core;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SolaceTK.Core.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CoreController : ControllerBase
    {
        public CoreContext Context { get; set; }

        public CoreController(CoreContext context)
        {
            Context = context;
        }


        #region Game Systems

        [HttpGet("gamesystems")]
        public ActionResult<IEnumerable<GameSystem>> GetSystems([FromQuery] bool includeElements = false)
        {
            if (!includeElements) return Ok(Context.GameSystems);

            return Ok(Context.GameSystems.ToListAsync());
        }

        [HttpGet("gamesystems/{name}")]
        public async Task<ActionResult<GameSystem>> GetSystem(string name)
        {
            return Ok(await Context.GameSystems.FirstOrDefaultAsync(x => x.Name == name));
        }

        [HttpPost("gamesystems")]
        public async Task<ActionResult<GameSystem>> CreateSystem([FromBody] GameSystem model)
        {
            if (model == null) return null;

            //model.Id = Guid.NewGuid();

            Context.GameSystems.Add(model);
            await Context.SaveChangesAsync();

            return Created($"/api/v1/core/gamesystems/{model.Id}", model);
        }

        [HttpPut("gamesystems/{id}")]
        public async Task<ActionResult<GameSystem>> UpdateSystem([FromBody] GameSystem model, int id)
        {
            if (model == null) return BadRequest();

            GameSystem tempModel = await Context.GameSystems.FirstOrDefaultAsync(x => x.Id == id);
            if (tempModel == null) return NotFound();

            // Apply Model Changes
            tempModel.BehaviorType = model.BehaviorType;
            tempModel.Description = model.Description;
            tempModel.Id = model.Id;
            tempModel.Name = model.Name;
            tempModel.Tags = model.Tags;

            await Context.SaveChangesAsync();

            return Created($"/api/v1/core/gamesystems/{model.Id}", model);
        }

        [HttpDelete("gamesystems/{id}")]
        public async Task<ActionResult> DeleteSystem(int id)
        {
            var imm = await Context.GameSystems.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (imm == null) return NotFound();

            Context.GameSystems.Remove(imm);
            await Context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("gamesystems/{id}/export")]
        public async Task<ActionResult> ExportSystem(int id)
        {
            var model = await Context.GameSystems.FirstOrDefaultAsync(x => x.Id == id);
            if (model == null) return NotFound();

            var json = JsonSerializer.Serialize(model);
            var bytes = Encoding.UTF8.GetBytes(json);

            return File(bytes, "application/json", fileDownloadName: $"{model.Name}.json");
        }

        #endregion


        #region Resource Collections

        [HttpGet("resourcecollections")]
        public ActionResult<IEnumerable<ResourceCollection>> GetCollections([FromQuery] bool includeElements = false)
        {
            if (!includeElements) return Ok(Context.Collections);

            return Ok(Context.Collections);
        }

        [HttpGet("resourcecollections/{name}")]
        public async Task<ActionResult<ResourceCollection>> GetCollection(string name)
        {
            return Ok(await Context.GameSystems.FirstOrDefaultAsync(x => x.Name == name));
        }

        [HttpPost("resourcecollections")]
        public async Task<ActionResult<ResourceCollection>> CreateCollection([FromBody] ResourceCollection model)
        {
            if (model == null) return null;


            Context.Collections.Add(model);
            await Context.SaveChangesAsync();

            return Created($"/api/v1/core/resourcecollections/{model.Id}", model);
        }

        [HttpPut("resourcecollections/{id}")]
        public async Task<ActionResult<ResourceCollection>> UpdateCollection([FromBody] ResourceCollection model, int id)
        {
            if (model == null) return BadRequest();

            ResourceCollection tempModel = await Context.Collections.FirstOrDefaultAsync(x => x.Id == id);
            if (tempModel == null) return NotFound();

            tempModel.Description = model.Description;
            tempModel.Id = model.Id;
            tempModel.Name = model.Name;
            tempModel.Tags = model.Tags;

            await Context.SaveChangesAsync();

            return Created($"/api/v1/core/resourcecollections/{model.Id}", model);
        }

        [HttpDelete("resourcecollections/{id}")]
        public async Task<ActionResult> DeleteCollection(int id)
        {
            var imm = await Context.Collections.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (imm == null) return NotFound();

            Context.Collections.Remove(imm);
            await Context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("resourcecollections/{id}/export")]
        public async Task<ActionResult> ExportCollection(int id)
        {
            var model = await Context.Collections.FirstOrDefaultAsync(x => x.Id == id);
            if (model == null) return NotFound();

            var json = JsonSerializer.Serialize(model);
            var bytes = Encoding.UTF8.GetBytes(json);

            return File(bytes, "application/json", fileDownloadName: $"{model.Name}.json");
        }

        #endregion


    }
}
