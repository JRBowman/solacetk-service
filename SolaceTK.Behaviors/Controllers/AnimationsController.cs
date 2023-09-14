using Microsoft.AspNetCore.Mvc;
using System.Text;
using SolaceTK.Models.Behavior;
using SolaceTK.Models.Telemetry;
using System.Text.Json;
using SolaceTK.Data.Services;

namespace SolaceTK.Behaviors.Controllers
{
    [Route("api/v1/Behaviors/[controller]")]
    [ApiController]
    public class AnimationsController : ControllerBase
    {
        private AnimationService _service;

        public AnimationsController(AnimationService service)
        {
            _service = service;
        }

        // GET: api/Animation
        [HttpGet]
        public async Task<ActionResult<ISolTkOperation<IEnumerable<Animation>>>> GetAnimations([FromQuery] bool noop = false)
        {
            return await _service.GetAsync();
        }

        // GET: api/Animation/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ISolTkOperation<Animation>>> GetAnimation(int id, [FromQuery] bool noop = false)
        {
            return await _service.GetAsync(id);
        }

        // POST: api/Animation
        [HttpPost]
        public async Task<ActionResult<ISolTkOperation<Animation>>> PostAnimation(Animation model)
        {
            return await _service.CreateAsync(model);
        }

        // PUT: api/Animation/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ISolTkOperation<Animation>>> PutAnimation(int id, Animation model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            return await _service.UpdateAsync(model);
        }

        // DELETE: api/Animation/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ISolTkOperation<bool>>> DeleteAnimation(int id)
        {
            var model = await _service.GetAsync(id);

            if (model.Data == null || model.ResultCode != SolTkOperationResultCode.Ok)
            {
                return new SolTkOperation<bool>(model.Name ?? "opp") { Status = model.Status, ResultCode = model.ResultCode };
            }

            return await _service.DeleteAsync(model.Data);
        }

        [HttpGet("{id}/export")]
        public async Task<ActionResult> ExportState(int id)
        {
            var model = await _service.GetAsync(id);
            if (model == null) return NotFound();

            var json = JsonSerializer.Serialize(model);
            var bytes = Encoding.UTF8.GetBytes(json);

            return File(bytes, "application/json", fileDownloadName: $"{model.Name}.json");
        }
    }
}