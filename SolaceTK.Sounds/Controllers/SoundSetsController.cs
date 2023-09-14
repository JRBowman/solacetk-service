using Microsoft.AspNetCore.Mvc;
using SolaceTK.Data.Services;
using SolaceTK.Data.Base;
using SolaceTK.Models.Environment;
using SolaceTK.Models.Sound;

namespace SolaceTK.Behaviors.Controllers
{
    [Route("api/v1/Sounds/[controller]")]
    public class SetsController : SolTkControllerBase<SoundSet>
    {
        private SoundSetService _service;

        public SetsController(SoundSetService service) : base(service)
        {
            _service = service;
        }

    }
}