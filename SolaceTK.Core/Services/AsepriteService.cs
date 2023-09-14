using Microsoft.AspNetCore.Http;
using SolaceTK.Core.Models;
using SolaceTK.Core.Models.Files;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SolaceTK.Core.Services
{
    public class AsepriteService
    {
        public string AsepriteCli { get; set; }
        public string ContentDirectory { get; set; } = "Ase";

        public List<string> AseStages = new() { "-act", "-start", "-end" };

        public AsepriteService(string asepriteCli, string contentDir)
        {
            AsepriteCli = asepriteCli;
            ContentDirectory = contentDir;
        }

        #region New File Operations:

        public async Task<AseFile> SaveAseFile(IFormFile file, AseImportOptions options)
        {
            Console.WriteLine($"File: {file.Name} - {file.ContentType} acquired...");
            var aseDir = file.Name.Split('.').First();

            var aseStage = AseStages.FirstOrDefault(x => aseDir.Contains(x));
            if (!string.IsNullOrEmpty(aseStage))
            {
                aseDir = aseDir.Replace(aseStage, "");
            }

            Console.WriteLine("Saving ASE to Disk...");
            return await FileService.EstablishFile(file, ContentDirectory, aseDir);
        }

        public async Task<AseFile> ConvertAseToSheet(AseFile file, AseImportOptions options)
        {
            var result = await Command("-b", "--list-layers", "--list-slices", 
                $"{file.Directory}/{file.AseName}",
                $"--sheet {file.Directory}/{file.SheetName}",
                $"--sheet-type {AseImportOptions.AseSheetTypeStings[(int)options.SheetType]}",
                $"--data {file.Directory}/{file.Name}.json");

            Console.WriteLine(result);

            return file;
        }

        public async Task<AseFile> ConvertAseToGif(AseFile file, AseImportOptions options)
        {
            var result = await Command("-b", $"{file.Directory}/{file.AseName}",
                $"--save-as {file.Directory}/{file.GifName}");
                //$"--data {file.Directory}/{file.Name}.json");

            Console.WriteLine(result);

            return file;
        }

        public async Task<AseFile> ConvertAseToLayers(AseFile file, AseImportOptions options)
        {
            var result = await Command("-b", "--split-layers", 
                $"{file.Directory}/{file.AseName}", 
                $"--save-as {file.Directory}/{file.Name}" + "-{layer}-{frame}.png");

            //var result2 = await ConvertAseToSheet(file, options);

            return file;
        }

        public async Task<AseFile> ConvertAseToTileSet(AseFile file, AseImportOptions options)
        {
            var result = await Command("-b", "--list-layers", "--list-slices", 
                $"{file.Directory}/{file.AseName}",
                $"--sheet {file.Directory}/{file.SheetName}",
                $"--sheet-type {AseImportOptions.AseSheetTypeStings[(int)options.SheetType]}",
                $"--data {file.Directory}/{file.Name}.json");

            return file;
        }

        #endregion

        #region Existing File Opertions:

        // Convert ASE To GIF:
        public async Task<string> ConvertAseToGif(string path, string aseFile)
        {
            var strReplace = aseFile.Split('.');
            var desiredFile = aseFile.Replace(strReplace.Last(), "gif");

            var tempPath = ContentDirectory;
            if (!string.IsNullOrEmpty(path)) tempPath += $"/{path}";

            var result = await Command("-b", $"{tempPath}/{aseFile}", $"--save-as {tempPath}/{desiredFile}", $"--data {tempPath}/{desiredFile}.json");
            Console.WriteLine(result);

            return $"{tempPath}/{desiredFile}";
        }

        // Convert ASE to PNG Sprite Sheet:
            public async Task<string> ConvertAseToSheet(string path, string aseFile, string sheetType = "rows")
        {
            var strReplace = aseFile.Split('.');
            var desiredFile = aseFile.Replace(strReplace.Last(), "png");

            var tempPath = ContentDirectory;
            if (!string.IsNullOrEmpty(path)) tempPath += $"/{path}";

            //path = string.IsNullOrEmpty(path) ? ContentDirectory + $"/{strReplace.First()}" : path;
            //if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            var result = await Command("-b", $"{tempPath}/{aseFile}", 
                $"--sheet {tempPath}/{desiredFile}", 
                $"--sheet-type {sheetType}",
                $"--data {tempPath}/{desiredFile}.json");

            return $"{tempPath}/{desiredFile}";
        }


        // Convert PNG Sprite Sheet to ASE:
        public async Task<string> ConvertSheetToAse(string path, string aseFile)
        {
            return "not yet implemented";
        }

        // Convert GIF to ASE:
        public async Task<string> ConvertGifToAse(string path, string aseFile)
        {
            return "not yet implemented";
        }

        #endregion


        #region Helper Operations:

        public async Task<string> GetVersion()
        {
            return await (Command("--version"));
        }

        #endregion

        #region Internal Command Execution:

        private async Task<string> Command(params string[] args)
        {
            string rawResult = "";

            using (Process proc = new())
            {
                proc.StartInfo.FileName = AsepriteCli;
                proc.StartInfo.Arguments = string.Join(' ', args);
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.Start();

                rawResult = await proc.StandardOutput.ReadToEndAsync();

                await proc.WaitForExitAsync();
            }

            return rawResult;
        }

        #endregion

    }
}
