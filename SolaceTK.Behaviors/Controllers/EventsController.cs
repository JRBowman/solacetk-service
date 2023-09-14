using Microsoft.AspNetCore.Mvc;
using System.Text;
using SolaceTK.Models.Telemetry;
using System.Text.Json;
using SolaceTK.Data.Services;
using SolaceTK.Models.Events;

namespace SolaceTK.Behaviors.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private EventService _service;

        public EventsController(EventService service)
        {
            _service = service;
        }

        // GET: api/SolTkEvent
        [HttpGet]
        public async Task<ActionResult<ISolTkOperation<IEnumerable<SolTkEvent>>>> GetSolTkEvents()
        {
            return await _service.GetAsync();
        }

        // GET: api/SolTkEvent/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ISolTkOperation<SolTkEvent>>> GetSolTkEvent(int id)
        {
            return await _service.GetAsync(id);
        }

        // POST: api/SolTkEvent
        [HttpPost]
        public async Task<ActionResult<ISolTkOperation<SolTkEvent>>> PostSolTkEvent(SolTkEvent model)
        {
            return await _service.CreateAsync(model);
        }

        // PUT: api/SolTkEvent/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ISolTkOperation<SolTkEvent>>> PutSolTkEvent(int id, SolTkEvent model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            return await _service.UpdateAsync(model);
        }

        // DELETE: api/SolTkEvent/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ISolTkOperation<bool>>> DeleteSolTkEvent(int id)
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