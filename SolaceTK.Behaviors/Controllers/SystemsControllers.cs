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
    public class SystemsController : ControllerBase
    {
        private BehaviorService _service;

        public SystemsController(BehaviorService service)
        {
            _service = service;
        }

        // GET: api/SolTkSystem
        [HttpGet]
        public async Task<ActionResult<ISolTkOperation<IEnumerable<SolTkSystem>>>> GetSolTkSystems()
        {
            return await _service.GetAsync();
        }

        // GET: api/SolTkSystem/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ISolTkOperation<SolTkSystem>>> GetSolTkSystem(int id)
        {
            return await _service.GetAsync(id);
        }

        // POST: api/SolTkSystem
        [HttpPost]
        public async Task<ActionResult<ISolTkOperation<SolTkSystem>>> PostSolTkSystem(SolTkSystem model)
        {
            return await _service.CreateAsync(model);
        }

        // PUT: api/SolTkSystem/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ISolTkOperation<SolTkSystem>>> PutSolTkSystem(int id, SolTkSystem model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            return await _service.UpdateAsync(model);
        }

        // DELETE: api/SolTkSystem/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ISolTkOperation<bool>>> DeleteSolTkSystem(int id)
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