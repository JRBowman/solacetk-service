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
    public class StatesController : ControllerBase
    {
        private StateService _service;

        public StatesController(StateService service)
        {
            _service = service;
        }

        // GET: api/SolTkState
        [HttpGet]
        public async Task<ActionResult<ISolTkOperation<IEnumerable<SolTkState>>>> GetSolTkStates([FromQuery] bool noop = false)
        {
            return await _service.GetAsync();
        }

        // GET: api/SolTkState/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ISolTkOperation<SolTkState>>> GetSolTkState(int id, [FromQuery] bool noop = false)
        {
            return await _service.GetAsync(id);
        }

        // POST: api/SolTkState
        [HttpPost]
        public async Task<ActionResult<ISolTkOperation<SolTkState>>> PostSolTkState(SolTkState state)
        {
            return await _service.CreateAsync(state);
        }

        // PUT: api/SolTkState/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ISolTkOperation<SolTkState>>> PutSolTkState(int id, SolTkState state)
        {
            if (id != state.Id)
            {
                return BadRequest();
            }

            return await _service.UpdateAsync(state);
        }

        // DELETE: api/SolTkState/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ISolTkOperation<bool>>> DeleteSolTkState(int id)
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