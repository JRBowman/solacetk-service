using Microsoft.AspNetCore.Mvc;
using SolaceTK.Core.Models;
using SolaceTK.Core.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using SolaceTK.Core.Models.Files;

namespace SolaceTK.Core.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {

        public AsepriteService AsepriteService { get; private set; }

        public FilesController(AsepriteService aseService)
        {
            AsepriteService = aseService;
        }

        // Import .ASE:
        [HttpPost("ase"), DisableRequestSizeLimit]
        public async Task<ActionResult> ImportAse([FromQuery]AseImportOptions options)
        {
            try
            {
                // Save the .ASE file to Storage:
                Console.WriteLine("Starting ASE Import...");
                var form = await Request.ReadFormAsync();
                var file = form.Files.First();
                if (file == null) return BadRequest();

                // Save Ase and Convert to Sheet:
                var aseFile = await AsepriteService.SaveAseFile(file, options);

                await AsepriteService.ConvertAseToSheet(aseFile, options);
                await AsepriteService.ConvertAseToGif(aseFile, options);

                if (options.SplitLayers) await AsepriteService.ConvertAseToLayers(aseFile, options);

                return Ok(new { data = new { previewUrl = $"http://{Request.Host}/{aseFile.Directory}/{aseFile.GifName}", 
                    sheetData = $"http://{Request.Host}/{aseFile.Directory}/{aseFile.Name}.json",
                    sheetUrl = $"http://{Request.Host}/{aseFile.Directory}/{aseFile.SheetName}" } });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Created($"/api/v1/files/ase", new{ });
        }

        [HttpPost(), DisableRequestSizeLimit]
        public async Task<ActionResult> ImportFiles()
        {
            List<FileResult> importedFiles = new();
            try
            {
                // Save the .ASE file to Storage:
                Console.WriteLine("Starting Files Import...");
                var form = await Request.ReadFormAsync();

                foreach(var file in form.Files)
                {
                    if (file == null) return BadRequest();
                    var aseDir = file.Name.Split('.').First();
                    importedFiles.Add(FileResult.FromFile(await FileService.EstablishFile(file, AsepriteService.ContentDirectory, aseDir), Request.Host.Value));
                }

                return Ok(new { data = importedFiles });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            return Created($"/api/v1/files", new{ });
        }
    }

    public class FileResult
    {
        public string PreviewUrl { get; set; }
        public string SheetData { get; set; }
        public string SheetUrl { get; set; }

        public static FileResult FromFile(AseFile model, string requestUrl)
        {
            return new FileResult()
            {
                PreviewUrl = $"http://{requestUrl}/{model.Directory}/{model.GifName}",
                SheetData = $"http://{requestUrl}/{model.Directory}/{model.Name}.json",
                SheetUrl = $"http://{requestUrl}/{model.Directory}/{model.SheetName}"
            };
        }
    }
}
