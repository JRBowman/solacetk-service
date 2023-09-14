using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SolaceTK.Models.Telemetry;

namespace SolaceTK.Telemetry.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TelemetryController : ControllerBase
    {
        public TelemetryController() { }

        /// <summary>
        /// Query Telemetry Data.
        /// </summary>
        /// <returns>Telemetry Report from the query.</returns>
        [HttpGet]
        public async Task<ActionResult<SolTkTelemetryReport>> Get()
        {
            var telemReport = new SolTkTelemetryReport();



            return Ok(telemReport);
        }
    }
}
