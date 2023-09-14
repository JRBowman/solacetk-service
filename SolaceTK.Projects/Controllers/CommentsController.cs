using Microsoft.AspNetCore.Mvc;
using SolaceTK.Data.Services;
using SolaceTK.Models.WorkItems;
using SolaceTK.Data.Base;

namespace SolaceTK.Behaviors.Controllers
{
    [Route("api/v1/Work/[controller]")]
    public class CommentsController : SolTkControllerBase<WorkComment>
    {
        private CommentService _service;

        public CommentsController(CommentService service) : base(service)
        {
            _service = service;
        }

    }
}