using Microsoft.AspNetCore.Mvc;
using SolaceTK.Data.Services;
using SolaceTK.Models.WorkItems;
using SolaceTK.Data.Base;

namespace SolaceTK.Behaviors.Controllers
{
    [Route("api/v1/Work/[controller]")]
    public class ItemsController : SolTkControllerBase<WorkItem>
    {
        private WorkItemService _service;

        public ItemsController(WorkItemService service) : base(service)
        {
            _service = service;
        }

    }
}