using SolaceTK.Data.Contexts;
using SolaceTK.Data;
using SolaceTK.Models.Environment;
using SolaceTK.Models.Telemetry;
using Microsoft.EntityFrameworkCore;
using SolaceTK.Models.Artifacts;

namespace SolaceTK.Data.Services
{
    public class EnvironmentService : ISolaceService<Map, int>
    {

        private EnvironmentContext _context;

        public IQueryable<Map> BaseQuery => _context.Maps;
        public IQueryable<Map> AllQuery => _context.Maps.Include(x => x.Layers).ThenInclude(x => x.LayerData)
                    .Include(x => x.TileSet)
                   .Include(x => x.Cells).ThenInclude(x => x.ActiveData)
                   .Include(x => x.Cells).ThenInclude(x => x.BehaviorEvents).ThenInclude(x => x.Conditions)
                   .Include(x => x.Cells).ThenInclude(x => x.BehaviorEvents).ThenInclude(x => x.DownstreamData)
                   .Include(x => x.Cells).ThenInclude(x => x.BehaviorEvents).ThenInclude(x => x.Messages)
                   .Include(x => x.Cells).ThenInclude(x => x.BehaviorEvents).ThenInclude(x => x.Messages).ThenInclude(x => x.Data)
                   .Include(x => x.Chunks).ThenInclude(x => x.Cells);

        public EnvironmentService(EnvironmentContext context)
        {
            _context = context;
        }

        #region Async Methods

        public async Task<SolTkOperation<IEnumerable<Map>>> GetAsync()
        {
            var operation = new SolTkOperation<IEnumerable<Map>>("GetMapsAsync");

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

        public async Task<SolTkOperation<Map>> GetAsync(int index)
        {
            var operation = new SolTkOperation<Map>("GetMapAsync");

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

        public async Task<SolTkOperation<Map>> GetAsync(string name)
        {
            var operation = new SolTkOperation<Map>("GetMapAsync");

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

        public async Task<SolTkOperation<Map>> CreateAsync(Map model)
        {
            var operation = new SolTkOperation<Map>("CreateMapAsync");

            operation.Start();

            try
            {
                var entry = _context.Maps.Add(model);
                var saves = await _context.SaveChangesAsync();
                operation.Status.AddLogs($"Maps Saved: {saves} Entities.");
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

        public IQueryable<Map> Query()
        {
            return _context.Maps;
        }

        public async Task<SolTkOperation<Map>> UpdateAsync(Map model)
        {
            var operation = new SolTkOperation<Map>("UpdateMapAsync");

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

                operation.Status.AddLogs($"Maps Saved: {saves} Entities.");
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


        public async Task<SolTkOperation<bool>> DeleteAsync(Map model)
        {
            var operation = new SolTkOperation<bool>("CreateMapAsync");

            operation.Start();

            try
            {
                _context.Maps.Remove(model);
                var saves = await _context.SaveChangesAsync();
                operation.Status.AddLogs($"Maps Saved: {saves} Entities.");
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