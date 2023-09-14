using Microsoft.AspNetCore.Mvc;
using SolaceTK.Data.Services;
using SolaceTK.Data.Base;
using SolaceTK.Models.Environment;
using SolaceTK.Models.Sound;
using SolaceTK.Models.Telemetry;

namespace SolaceTK.Behaviors.Controllers
{
    [Route("api/v1/Sounds/[controller]")]
    public class SourcesController : SolTkControllerBase<SoundSource>
    {
        private SoundService _service;

        public SourcesController(SoundService service) : base(service)
        {
            _service = service;
        }

    }
}