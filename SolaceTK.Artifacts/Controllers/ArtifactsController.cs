using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SolaceTK.Data.Base;
using SolaceTK.Data.Services;
using SolaceTK.Models.Artifacts;
using SolaceTK.Models.Telemetry;

namespace SolaceTK.Behaviors.Controllers
{
    [Route("api/v1/[controller]")]
    public class ArtifactsController : SolTkControllerBase<SolTkArtifact>
    {
        private ArtifactService _service;

        public ArtifactsController(ArtifactService service) : base(service)
        {
            _service = service;
        }

        // Import .ASE:
        [HttpPost("Instances"), DisableRequestSizeLimit]
        public async Task<SolTkOperation<SolTkArtifact>> ImportArtifact([FromQuery]string? collectionRoot = null)
        {
            SolTkOperation<SolTkArtifact> operation = new SolTkOperation<SolTkArtifact>();
            SolTkArtifact model = new SolTkArtifact();
            try
            {
                // Save the .ASE file to Storage:
                var form = await Request.ReadFormAsync();
                var file = form.Files.First();

                operation = model.Id == 0
                    ? await _service.CreateAsync(model, file, collectionRoot)
                    : await _service.UpdateAsync(model, file, collectionRoot);

                if (operation.ResultCode == SolTkOperationResultCode.NoOp) operation.Status.AddErrors($"Artifact: {model.Name} - Operation was not successful, review the logs and try again.");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                operation.Status.AddException(ex);
                return operation;
            }

            return operation;
        }

    }
}