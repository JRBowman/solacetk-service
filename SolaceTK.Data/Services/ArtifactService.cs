using SolaceTK.Models.WorkItems;
using SolaceTK.Data.Contexts;
using SolaceTK.Data;
using SolaceTK.Models.Telemetry;
using SolaceTK.Models.Artifacts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace SolaceTK.Data.Services
{
    public class ArtifactService : ISolaceService<SolTkArtifact, int>
    {

        private CoreContext _context;
        private SolTkFileService _fileService;

        public IQueryable<SolTkArtifact> BaseQuery => _context.Attachments;
        public IQueryable<SolTkArtifact> AllQuery => BaseQuery;

        public ArtifactService(CoreContext context, SolTkFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }


        #region Async Methods

        public async Task<SolTkOperation<IEnumerable<SolTkArtifact>>> GetAsync()
        {
            var operation = new SolTkOperation<IEnumerable<SolTkArtifact>>("GetArtifactsAsync");

            operation.Start();

            try
            {
                operation.Data = await AllQuery.ToListAsync();

                operation.ResultCode = SolTkOperationResultCode.Ok;
            }
            catch (Exception ex)
            {
                operation.Status.AddException(ex);
                operation.ResultCode = SolTkOperationResultCode.ExThrown;
            }

            operation.Stop();

            return operation;
        }

        public async Task<SolTkOperation<SolTkArtifact>> GetAsync(int index)
        {
            var operation = new SolTkOperation<SolTkArtifact>("GetArtifactAsync");

            operation.Start();

            try
            {
                operation.Data = await AllQuery.FirstOrDefaultAsync(x => x.Id == index);

                operation.ResultCode = SolTkOperationResultCode.Ok;
            }
            catch (Exception ex)
            {
                operation.Status.AddException(ex);
                operation.ResultCode = SolTkOperationResultCode.ExThrown;
            }

            operation.Stop();

            return operation;
        }

        public async Task<SolTkOperation<SolTkArtifact>> GetAsync(string name)
        {
            var operation = new SolTkOperation<SolTkArtifact>("GetArtifactAsync");

            operation.Start();

            try
            {
                operation.Data = await AllQuery.FirstOrDefaultAsync(x => x.Name == name);

                operation.ResultCode = SolTkOperationResultCode.Ok;
            }
            catch (Exception ex)
            {
                operation.Status.AddException(ex);
                operation.ResultCode = SolTkOperationResultCode.ExThrown;
            }

            operation.Stop();

            return operation;
        }


        public async Task<SolTkOperation<SolTkArtifact>> CreateAsync(SolTkArtifact model)
        {
            var operation = new SolTkOperation<SolTkArtifact>("CreateArtifactAsync");

            operation.Start();

            try
            {
                // Use FileService to Save the file:


                var entry = _context.Add(model);
                var saves = await _context.SaveChangesAsync();
                operation.Status.AddLogs($"Artifacts Saved: {saves} Entities.");
                if (saves > 0) operation.ResultCode = SolTkOperationResultCode.Created;
                if (saves == 0) operation.ResultCode = SolTkOperationResultCode.NoOp;

                operation.Data = entry.Entity;
            }
            catch (Exception ex)
            {
                operation.Status.AddException(ex);
                operation.ResultCode = SolTkOperationResultCode.ExThrown;
            }

            operation.Stop();

            return operation;
        }

        public async Task<SolTkOperation<SolTkArtifact>> CreateAsync(SolTkArtifact model, IFormFile artifact, string? collectionRoot = null)
        {
            var operation = new SolTkOperation<SolTkArtifact>("CreateArtifactAsync");

            operation.Start();

            try
            {
                operation.Status.AddLogs($"ContentType: {artifact.ContentType}");
                //operation.Status.AddLogs(artifact.Headers.Values.Select(x => string.Join(",", x.ToArray())).ToArray());

                // Use FileService to Save the file:
                var innerOperation = await _fileService.CreateAsync(model, artifact, collectionRoot);

                if (innerOperation != null && innerOperation.ResultCode == SolTkOperationResultCode.Failed)
                {
                    // Set Inner Data to Operation:
                    operation.Status.Errors = innerOperation.Status.Errors;
                    operation.Status.Logs = innerOperation.Status.Logs;
                    operation.Status.Exceptions = innerOperation.Status.Exceptions;
                }

                if (innerOperation != null && innerOperation.Data != null)
                {
                    model = innerOperation.Data;
                }

                var entry = _context.Add(model);
                var saves = await _context.SaveChangesAsync();
                operation.Status.AddLogs($"Artifacts Saved: {saves} Entities.");
                if (saves > 0) operation.ResultCode = SolTkOperationResultCode.Created;
                if (saves == 0) operation.ResultCode = SolTkOperationResultCode.NoOp;

                operation.Data = entry.Entity;
            }
            catch (Exception ex)
            {
                operation.Status.AddException(ex);
                operation.ResultCode = SolTkOperationResultCode.ExThrown;
            }

            operation.Stop();

            return operation;
        }

        public IQueryable<SolTkArtifact> Query()
        {
            return _context.Attachments;
        }

        public async Task<SolTkOperation<SolTkArtifact>> UpdateAsync(SolTkArtifact model)
        {
            var operation = new SolTkOperation<SolTkArtifact>("UpdateArtifactAsync");

            operation.Start();

            try
            {
                // Get Existing Entity:
                var entity = await GetAsync(model.Id);
                if (entity.ResultCode != SolTkOperationResultCode.Ok)
                {
                    operation.Status.AddErrors("The Model Provided for update wasn't found - Check Id/Data and submit again.");
                    operation.Stop();
                    return operation;
                }

                // Merge Entity and Model:
                entity.Data?.Merge(model);
                var saves = await _context.SaveChangesAsync();

                operation.Status.AddLogs($"Artifacts Saved: {saves} Entities.");
                if (saves > 0) operation.ResultCode = SolTkOperationResultCode.Updated;
                if (saves == 0) operation.ResultCode = SolTkOperationResultCode.NoOp;

                operation.Data = entity.Data;
            }
            catch (Exception ex)
            {
                operation.Status.AddException(ex);
                operation.ResultCode = SolTkOperationResultCode.ExThrown;
            }

            operation.Stop();

            return operation;
        }

        public async Task<SolTkOperation<SolTkArtifact>> UpdateAsync(SolTkArtifact model, IFormFile artifact, string? collectionRoot = null)
        {
            var operation = new SolTkOperation<SolTkArtifact>("UpdateArtifactAsync");

            operation.Start();

            try
            {
                // Get Existing Entity:
                var entity = await GetAsync(model.Id);
                if (entity.ResultCode != SolTkOperationResultCode.Ok)
                {
                    operation.Status.AddErrors("The Model Provided for update wasn't found - Check Id/Data and submit again.");
                    operation.Stop();
                    return operation;
                }

                // Use FileService to Save the file:
                var innerOperation = await _fileService.UpdateAsync(model, artifact, collectionRoot);

                if (innerOperation != null && innerOperation.ResultCode == SolTkOperationResultCode.Failed)
                {
                    // Set Inner Data to Operation:
                    operation.Status.Errors = innerOperation.Status.Errors;
                    operation.Status.Logs = innerOperation.Status.Logs;
                    operation.Status.Exceptions = innerOperation.Status.Exceptions;
                }

                // Merge Entity and Model:
                entity.Data?.Merge(innerOperation.Data);

                var saves = await _context.SaveChangesAsync();

                operation.Status.AddLogs($"Artifacts Saved: {saves} Entities.");
                if (saves > 0) operation.ResultCode = SolTkOperationResultCode.Updated;
                if (saves == 0) operation.ResultCode = SolTkOperationResultCode.NoOp;

                operation.Data = entity.Data;
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
            var operation = new SolTkOperation<bool>("CreateArtifactAsync");

            operation.Start();

            try
            {
                _context.Remove(model);
                var saves = await _context.SaveChangesAsync();
                operation.Status.AddLogs($"Artifacts Saved: {saves} Entities.");
                if (saves > 0)
                {
                    operation.Data = true;
                    operation.ResultCode = SolTkOperationResultCode.Deleted;
                }
                if (saves == 0)
                {
                    operation.Data = false;
                    operation.ResultCode = SolTkOperationResultCode.NoOp;
                }
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