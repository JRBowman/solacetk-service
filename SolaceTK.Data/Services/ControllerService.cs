using SolaceTK.Data.Contexts;
using SolaceTK.Data;
using SolaceTK.Models;
using SolaceTK.Models.Telemetry;
using Microsoft.EntityFrameworkCore;
using SolaceTK.Models.Artifacts;

namespace SolaceTK.Data.Services
{
    public class ControllerService : ISolaceService<SolTkController, int>
    {

        private ControllerContext _context;

        public IQueryable<SolTkController> BaseQuery => _context.Controllers;
        public IQueryable<SolTkController> AllQuery => _context.Controllers.Include(x => x.Components).ThenInclude(x => x.ComponentData)
                    .Include(x => x.SoundSet).ThenInclude(x => x.Sources).ThenInclude(x => x.SoundData);

        public ControllerService(ControllerContext context)
        {
            _context = context;
        }

        #region Async Methods

        public async Task<SolTkOperation<IEnumerable<SolTkController>>> GetAsync()
        {
            var operation = new SolTkOperation<IEnumerable<SolTkController>>("GetControllersAsync");

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

        public async Task<SolTkOperation<SolTkController>> GetAsync(int index)
        {
            var operation = new SolTkOperation<SolTkController>("GetControllerAsync");

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

        public async Task<SolTkOperation<SolTkController>> GetAsync(string name)
        {
            var operation = new SolTkOperation<SolTkController>("GetArtifactAsync");

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

        public async Task<SolTkOperation<SolTkController>> CreateAsync(SolTkController model)
        {
            var operation = new SolTkOperation<SolTkController>("CreateControllerAsync");

            operation.Start();

            try
            {
                var entry = _context.Controllers.Add(model);
                var saves = await _context.SaveChangesAsync();
                operation.Status.AddLogs($"States Saved: {saves} Entities.");
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

        public IQueryable<SolTkController> Query()
        {
            return _context.Controllers;
        }

        public async Task<SolTkOperation<SolTkController>> UpdateAsync(SolTkController model)
        {
            var operation = new SolTkOperation<SolTkController>("UpdateControllerAsync");

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

                operation.Status.AddLogs($"Controller Saved: {saves} Entities.");
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


        public async Task<SolTkOperation<bool>> DeleteAsync(SolTkController model)
        {
            var operation = new SolTkOperation<bool>("CreateControllerAsync");

            operation.Start();

            try
            {
                _context.Controllers.Remove(model);
                var saves = await _context.SaveChangesAsync();
                operation.Status.AddLogs($"Controllers Saved: {saves} Entities.");
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