using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolaceTK.Core.Contexts;
using SolaceTK.Core.Models.Sound;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolaceTK.Core.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SoundController : ControllerBase
    {
        public SoundContext Context { get; set; }

        public SoundController(SoundContext context)
        {
            Context = context;
        }

        #region Sets:

        [HttpGet("sets")]
        public async Task<ActionResult<IEnumerable<SoundSet>>> GetSets()
        {
            return Ok(Context.SoundSources);
        }

        [HttpGet("sets/{name}")]
        public async Task<ActionResult<SoundSource>> GetSet(string name)
        {
            return Ok(await Context.SoundSources.FirstOrDefaultAsync(x => x.Name == name));
        }

        [HttpPost("sets")]
        public async Task<ActionResult> CreateSet([FromBody] SoundSet model)
        {
            if (model == null) return null;

            Context.SoundSets.Add(model);
            await Context.SaveChangesAsync();

            return Created($"/api/v1/sound/sets/{model.Name}", model);
        }

        [HttpPut("sets/{id}")]
        public async Task<ActionResult> UpdateSet([FromBody] SoundSet model, int id)
        {
            var tempmodel = await Context.SoundSets.FirstOrDefaultAsync(x => x.Id == id);
            if (tempmodel == null) return NotFound();

            tempmodel = model;
            await Context.SaveChangesAsync();

            return Created($"/api/v1/sound/sets/{model.Name}", model);
        }

        [HttpDelete("sets/{id}")]
        public async Task<ActionResult> DeleteSet(int id)
        {
            var model = await Context.SoundSets.FirstOrDefaultAsync(x => x.Id == id);
            if (model == null) return NotFound();

            Context.SoundSets.Remove(model);
            await Context.SaveChangesAsync();

            return Created($"/api/v1/sound/sets/{model.Name}", model);
        }

        #endregion

        #region Sources:

        [HttpGet("sources")]
        public async Task<ActionResult<IEnumerable<SoundSource>>> GetSources()
        {
            return Ok(Context.SoundSources);
        }

        [HttpGet("sources/{name}")]
        public async Task<ActionResult<SoundSource>> GetSources(string name)
        {
            return Ok(await Context.SoundSources.FirstOrDefaultAsync(x => x.Name == name));
        }

        [HttpPost("sources")]
        public async Task<ActionResult> CreateSource([FromBody]SoundSource model)
        {
            if (model == null) return null;

            Context.SoundSources.Add(model);
            await Context.SaveChangesAsync();

            return Created($"/api/v1/sound/{model.Name}", model);
        }

        [HttpPut("sources/{id}")]
        public async Task<ActionResult> UpdateSource([FromBody] SoundSource model, int id)
        {
            var tempmodel = await Context.SoundSources.FirstOrDefaultAsync(x => x.Id == id);
            if (tempmodel == null) return NotFound();

            tempmodel = model;
            await Context.SaveChangesAsync();

            return Created($"/api/v1/sound/sources/{model.Name}", model);
        }

        [HttpDelete("sources/{id}")]
        public async Task<ActionResult> DeleteSource(int id)
        {
            var model = await Context.SoundSources.FirstOrDefaultAsync(x => x.Id == id);
            if (model == null) return NotFound();

            Context.SoundSources.Remove(model);
            await Context.SaveChangesAsync();

            return Created($"/api/v1/sound/sources/{model.Name}", model);
        }

        #endregion
    }
}
