using SolaceTK.Data.Contexts;
using SolaceTK.Data;
using SolaceTK.Models.Environment;
using SolaceTK.Models.Telemetry;
using Microsoft.EntityFrameworkCore;
using SolaceTK.Models.Artifacts;

namespace SolaceTK.Data.Services
{
    public class TileSetService : ISolaceService<TileSet, int>
    {

        private EnvironmentContext _context;

        public IQueryable<TileSet> BaseQuery => _context.TileSets;
        public IQueryable<TileSet> AllQuery => _context.TileSets.Include(x => x.Tiles).ThenInclude(x => x.Rules);

        public TileSetService(EnvironmentContext context)
        {
            _context = context;
        }

        #region Async Methods

        public async Task<SolTkOperation<IEnumerable<TileSet>>> GetAsync()
        {
            var operation = new SolTkOperation<IEnumerable<TileSet>>("GetTileSetsAsync");

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

        public async Task<SolTkOperation<TileSet>> GetAsync(int index)
        {
            var operation = new SolTkOperation<TileSet>("GetTileSetAsync");

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

        public async Task<SolTkOperation<TileSet>> GetAsync(string name)
        {
            var operation = new SolTkOperation<TileSet>("GetTileSetAsync");

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

        public async Task<SolTkOperation<TileSet>> CreateAsync(TileSet model)
        {
            var operation = new SolTkOperation<TileSet>("CreateTileSetAsync");

            operation.Start();

            try
            {
                var entry = _context.TileSets.Add(model);
                var saves = await _context.SaveChangesAsync();
                operation.Status.AddLogs($"TileSets Saved: {saves} Entities.");
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

        public IQueryable<TileSet> Query()
        {
            return _context.TileSets;
        }

        public async Task<SolTkOperation<TileSet>> UpdateAsync(TileSet model)
        {
            var operation = new SolTkOperation<TileSet>("UpdateTileSetAsync");

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

                operation.Status.AddLogs($"TileSets Saved: {saves} Entities.");
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


        public async Task<SolTkOperation<bool>> DeleteAsync(TileSet model)
        {
            var operation = new SolTkOperation<bool>("CreateTileSetAsync");

            operation.Start();

            try
            {
                _context.TileSets.Remove(model);
                var saves = await _context.SaveChangesAsync();
                operation.Status.AddLogs($"TileSets Saved: {saves} Entities.");
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