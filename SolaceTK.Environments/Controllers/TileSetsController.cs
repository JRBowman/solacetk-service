using Microsoft.AspNetCore.Mvc;
using SolaceTK.Data.Services;
using SolaceTK.Data.Base;
using SolaceTK.Models.Environment;

namespace SolaceTK.Behaviors.Controllers
{
    [Route("api/v1/Environments/[controller]")]
    public class TileSetsController : SolTkControllerBase<TileSet>
    {
        private TileSetService _service;

        public TileSetsController(TileSetService service) : base(service)
        {
            _service = service;
        }

    }
}