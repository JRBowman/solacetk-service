using Microsoft.AspNetCore.Mvc;
using SolaceTK.Data.Services;
using SolaceTK.Models.WorkItems;
using SolaceTK.Data.Base;

namespace SolaceTK.Behaviors.Controllers
{
    [Route("api/v1/Work/[controller]")]
    public class ProjectsController : SolTkControllerBase<WorkProject>
    {
        private ProjectService _service;

        public ProjectsController(ProjectService service) : base(service)
        {
            _service = service;
        }

    }
}