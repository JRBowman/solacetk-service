using Microsoft.AspNetCore.Mvc;
using SolaceTK.Data.Services;
using SolaceTK.Models.WorkItems;
using SolaceTK.Data.Base;
using SolaceTK.Models.Events;

namespace SolaceTK.Behaviors.Controllers
{
    [Route("api/v1/[controller]")]
    public class HubsController : SolTkControllerBase<SolTkEvent>
    {
        private EventService _service;

        public HubsController(EventService service) : base(service)
        {
            _service = service;
        }

    }
}