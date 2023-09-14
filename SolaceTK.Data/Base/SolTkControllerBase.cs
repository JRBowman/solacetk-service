using Microsoft.AspNetCore.Mvc;
using SolaceTK.Models.Telemetry;
using System.Text;
using System.Text.Json;

namespace SolaceTK.Data.Base
{
    [ApiController]
    public class SolTkControllerBase<T> : ControllerBase
    {
        private readonly ISolaceService<T, int> _service;

        public SolTkControllerBase(ISolaceService<T, int> service)
        {
            _service = service;
        }

        // GET: api/WorkProject
        [HttpGet]
        public async Task<ActionResult<ISolTkOperation<IEnumerable<T>>>> Get()
        {
            return await _service.GetAsync();
        }

        // GET: api/WorkProject/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ISolTkOperation<T>>> GetById(int id)
        {
            return await _service.GetAsync(id);
        }

        //// GET: api/WorkProject/5
        //[HttpGet("{name}")]
        //public async Task<ActionResult<ISolTkOperation<T>>> GetByName(string name)
        //{
        //    return await _service.GetAsync(name);
        //}

        // POST: api/WorkProject
        [HttpPost]
        public async Task<ActionResult<ISolTkOperation<T>>> PostWorkProject(T model)
        {
            return await _service.CreateAsync(model);
        }

        // PUT: api/WorkProject/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ISolTkOperation<T>>> PutWorkProject(int id, T model)
        {
            return await _service.UpdateAsync(model);
        }

        // DELETE: api/WorkProject/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ISolTkOperation<bool>>> DeleteWorkProject(int id)
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
