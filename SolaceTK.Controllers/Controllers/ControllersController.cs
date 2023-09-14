using Microsoft.AspNetCore.Mvc;
using SolaceTK.Data.Services;
using SolaceTK.Data.Base;
using SolaceTK.Models;

namespace SolaceTK.Behaviors.Controllers
{
    [Route("api/v1/[controller]")]
    public class ControllersController : SolTkControllerBase<SolTkController>
    {
        private ControllerService _service;

        public ControllersController(ControllerService service) : base(service)
        {
            _service = service;
        }

    }
}