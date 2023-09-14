using SolaceTK.Data.Contexts;
using SolaceTK.Data;
using SolaceTK.Models.Core;
using SolaceTK.Models.Telemetry;
using SolaceTK.Models.Artifacts;
using Microsoft.AspNetCore.Http;

namespace SolaceTK.Data.Services
{
    public class SolTkFileService
    {

        public string ContentDirectory { get; private set; }

        public List<string> PaddedExtensions { get; private set; } = new();

        public static Dictionary<string, List<string>> ExtensionsCollection = new Dictionary<string, List<string>>() 
        {
            
            { "html",  new() { "html", "htm", "shtml", "xhtml", "xht", "mdoc", "jsp", "asp", "aspx", "jshtm" } },
            { "python",  new() { "py", "rpy", "pyw", "cpy", "gyp", "gypi", "pyi", "ipy", "pyt" } },
            { "ruby",  new() { "rb", "rbx", "rjs", "gemspec", "rake", "ru", "erb", "podspec", "rbi" } },
            { "shaders", new() { "hlsl", "hlsli", "fx", "fxh", "vsh", "psh", "cginc", "compute" } },
            { "c++",  new() { "cpp", "cc", "cxx", "c++", "hpp", "h", "hh", "hxx", "h++", "ii" } },
            { "shell-script",  new() { "sh", "bash", "bashrc", "bash_aliases", "bash_profile" } },
            { "clojure",  new() { "clj", "cljs", "cljc", "cljx", "clojure", "edn" } },
            { "groovy",  new() { "groovy", "gvy", "gradle", "jenkinsfile", "nf" } },
            { "powershell",  new() { "ps1", "psm1", "psd1", "pssc", "psrc" } },
            { "perl",  new() { "pl", "pm", "pod", "t", "PL", "psgi" } },
            { "php",  new() { "php", "php4", "php5", "phtml", "ctp" } },
            { "dockerfile",  new() { "dockerfile", "containerfile" } },
            { "handlebars",  new() { "handlebars", "hbs", "hjs" } },
            { "javascript",  new() { "js", "es6", "mjs", "cjs" } },
            { "stylesheets",  new() { "css", "scss", "less" } },
            { "f#",  new() { "fs", "fsi", "fsx", "fsscript" } },
            { "asp.net-razor",  new() { "cshtml", "razor" } },
            { "typescript",  new() { "ts", "tsx" } },
            { "c#",  new() { "cs", "csx", "cake" } },
            { "cuda-c++",  new() { "cu", "cuh" } },
            { "yaml",  new() { "yml", "yaml" } },
            { "java",  new() { "java", "jav" } },
            { "sql",  new() { "sql", "dsql" } },
            { "batch",  new() { "bat", "cmd" } },
            { "plain-text",  new() { "txt" } },
            { "react",  new() { "jsx" } },
            { "c",  new() { "c", "i" } },
            { "lua",  new() { "lua" } },
            { "rust",  new() { "rs" } },
            { "go",  new() { "go" } },
        };

        public SolTkFileService(string contentDirectory)
        {
            ContentDirectory = contentDirectory;

            // Configure Padded Extenstions:
            var blockedExtensions = Environment.GetEnvironmentVariable("SOLTK_PADDED_EXTENSIONS")
                ?? "c++,yaml,sql,java,powershell,cuda-c++,shell-script,f#,c,go,rust,lua,perl,php,python,groovy,clojure,ruby";

            // Add all blocked extensions into the Padded Collection:
            foreach (var ext in blockedExtensions.Split(','))
            {
                PaddedExtensions.AddRange(ExtensionsCollection[ext]);
            }
        }

        #region Async Methods

        public async Task<SolTkOperation<IEnumerable<SolTkArtifact>>> GetAsync()
        {
            var operation = new SolTkOperation<IEnumerable<SolTkArtifact>>("GetFileAsync");

            operation.Start();

            try
            {

            }
            catch (Exception ex)
            {
                operation.Status.AddException(ex);
                operation.ResultCode = SolTkOperationResultCode.ExThrown;
            }

            operation.Stop();

            return operation;
        }

       

        public async Task<SolTkOperation<SolTkArtifact>> CreateAsync(SolTkArtifact artifact, IFormFile model, string? collectionRoot = null)
        {
            var operation = new SolTkOperation<SolTkArtifact>("CreateFileAsync");

            operation.Start();

            try
            {
                artifact.CollectionRoot = collectionRoot;

                // Establish the File:
                //operation.Status.AddLogs($"ContentType: {model.ContentType}", $"ContentDisposition: {model.ContentDisposition}");
                //operation.Status.AddLogs(model.Headers.Values.Select(x => string.Join(",", x.ToArray())).ToArray());

                operation.Data = await artifact.EstablishAsync(model, ContentDirectory);

                if (operation.Data != null) operation.ResultCode = SolTkOperationResultCode.Created;
                else operation.ResultCode = SolTkOperationResultCode.Failed;

            }
            catch (Exception ex)
            {
                operation.Status.AddException(ex);
                operation.ResultCode = SolTkOperationResultCode.ExThrown;
            }

            operation.Stop();

            return operation;
        }

      
        public async Task<SolTkOperation<SolTkArtifact>> UpdateAsync(SolTkArtifact artifact, IFormFile model, string? collectionRoot = null)
        {
            var operation = new SolTkOperation<SolTkArtifact>("UpdateFileAsync");

            operation.Start();

            try
            {
                artifact.CollectionRoot = collectionRoot;

                // Establish the File:
                //operation.Status.AddLogs($"ContentType: {model.ContentType}", $"ContentDisposition: {model.ContentDisposition}");
                //operation.Status.AddLogs(model.Headers.Values.Select(x => string.Join(",", x.ToArray())).ToArray());

                operation.Data = await artifact.EstablishAsync(model, ContentDirectory);

                if (operation.Data != null) operation.ResultCode = SolTkOperationResultCode.Updated;
                else operation.ResultCode = SolTkOperationResultCode.Failed;
            }
            catch (Exception ex)
            {
                operation.Status.AddException(ex);
                operation.ResultCode = SolTkOperationResultCode.ExThrown;
            }

            operation.Stop();

            return operation;
        }


        public async Task<SolTkOperation<bool>> DeleteAsync(SolTkArtifact model)
        {
            var operation = new SolTkOperation<bool>("DeleteFileAsync");

            operation.Start();

            try
            {
                // Establish the File:
                operation.Data = await model.DeleteArtifactAsync();

                if (operation.Data) operation.ResultCode = SolTkOperationResultCode.Deleted;
                else operation.ResultCode = SolTkOperationResultCode.Failed;
            }
            catch (Exception ex)
            {
                operation.Status.AddException(ex);
                operation.ResultCode = SolTkOperationResultCode.ExThrown;
            }

            operation.Stop();

            return operation;
        }

        #endregion
    }
}