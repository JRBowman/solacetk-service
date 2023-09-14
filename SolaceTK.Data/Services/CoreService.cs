using SolaceTK.Data.Contexts;
using SolaceTK.Data;
using SolaceTK.Models.Core;
using SolaceTK.Models.Telemetry;
using SolaceTK.Models.Artifacts;

namespace SolaceTK.Data.Services
{
    public class CoreService : ISolaceService<ResourceCollection, int>
    {

        private CoreContext _context;

        public IQueryable<ResourceCollection> BaseQuery => _context.Collections;
        public IQueryable<ResourceCollection> AllQuery => BaseQuery;

        public CoreService(CoreContext context)
        {
            _context = context;
        }

        #region Async Methods

        public async Task<SolTkOperation<IEnumerable<ResourceCollection>>> GetAsync()
        {
            var operation = new SolTkOperation<IEnumerable<ResourceCollection>>("GetResourceCollectionsAsync");

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

        public async Task<SolTkOperation<ResourceCollection>> GetAsync(int index)
        {
            var operation = new SolTkOperation<ResourceCollection>("GetResourceCollectionAsync");

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

        public async Task<SolTkOperation<ResourceCollection>> GetAsync(string name)
        {
            var operation = new SolTkOperation<ResourceCollection>("GetResourceCollectionAsync");

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

        public async Task<SolTkOperation<ResourceCollection>> CreateAsync(ResourceCollection model)
        {
            var operation = new SolTkOperation<ResourceCollection>("CreateResourceCollectionAsync");

            operation.Start();

            try
            {
                var entry = _context.Collections.Add(model);
                var saves = await _context.SaveChangesAsync();
                operation.Status.AddLogs($"Collections Saved: {saves} Entities.");
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

        public IQueryable<ResourceCollection> Query()
        {
            return _context.Collections;
        }

        public async Task<SolTkOperation<ResourceCollection>> UpdateAsync(ResourceCollection model)
        {
            var operation = new SolTkOperation<ResourceCollection>("UpdateResourceCollectionAsync");

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

                operation.Status.AddLogs($"Collections Saved: {saves} Entities.");
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


        public async Task<SolTkOperation<bool>> DeleteAsync(ResourceCollection model)
        {
            var operation = new SolTkOperation<bool>("CreateResourceCollectionAsync");

            operation.Start();

            try
            {
                _context.Collections.Remove(model);
                var saves = await _context.SaveChangesAsync();
                operation.Status.AddLogs($"Collections Saved: {saves} Entities.");
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