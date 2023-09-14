using Microsoft.AspNetCore.Mvc;
using SolaceTK.Data.Services;
using SolaceTK.Data.Base;
using SolaceTK.Models.Environment;

namespace SolaceTK.Behaviors.Controllers
{
    [Route("api/v1/Environments/[controller]")]
    public class MapsController : SolTkControllerBase<Map>
    {
        private EnvironmentService _service;

        public MapsController(EnvironmentService service) : base(service)
        {
            _service = service;
        }

    }
}