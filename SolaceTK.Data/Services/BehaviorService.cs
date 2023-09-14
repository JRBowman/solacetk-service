using System;
using System.Linq;
using SolaceTK.Models.Behavior;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SolaceTK.Data.Contexts;
using SolaceTK.Data;
using SolaceTK.Models.Telemetry;
using SolaceTK.Models.Artifacts;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SolaceTK.Data.Services
{
    public class BehaviorService : ISolaceService<SolTkSystem, int>
    {

        private BehaviorContext _context;

        public IQueryable<SolTkSystem> BaseQuery => _context.Systems;
        public IQueryable<SolTkSystem> AllQuery => _context.Systems.Include(x => x.VarData)
                .Include(x => x.Events).ThenInclude(x => x.Conditions)
                .Include(x => x.Events).ThenInclude(x => x.DownstreamData)
                .Include(x => x.Events).ThenInclude(x => x.Messages).ThenInclude(x => x.Data)
                .Include(x => x.Behaviors).ThenInclude(x => x.Events).ThenInclude(x => x.Conditions)
                .Include(x => x.Behaviors).ThenInclude(x => x.Events).ThenInclude(x => x.DownstreamData)
                .Include(x => x.Behaviors).ThenInclude(x => x.Events).ThenInclude(x => x.Messages).ThenInclude(x => x.Data)
                .Include(x => x.Behaviors).ThenInclude(x => x.Animation).ThenInclude(x => x.ActFrameData).ThenInclude(x => x.Frames).ThenInclude(x => x.DownstreamData)
                .Include(x => x.Behaviors).ThenInclude(x => x.StartData)
                .Include(x => x.Behaviors).ThenInclude(x => x.EndData)
                .Include(x => x.Behaviors).ThenInclude(x => x.Conditions)
                .Include(x => x.Behaviors).ThenInclude(x => x.NextStates)
                .Include(x => x.Behaviors).ThenInclude(x => x.ActData);

        public BehaviorService(BehaviorContext context)
        {
            _context = context;
        }

        #region Async Methods

        public async Task<SolTkOperation<IEnumerable<SolTkSystem>>> GetAsync()
        {
            var operation = new SolTkOperation<IEnumerable<SolTkSystem>>("GetSystemsAsync");

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

        public async Task<SolTkOperation<SolTkSystem>> GetAsync(int index)
        {
            var operation = new SolTkOperation<SolTkSystem>("GetSystemAsync");

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

        public async Task<SolTkOperation<SolTkSystem>> GetAsync(string name)
        {
            var operation = new SolTkOperation<SolTkSystem>("GetSystemAsync");
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

        public async Task<SolTkOperation<SolTkSystem>> CreateAsync(SolTkSystem model)
        {
            var operation = new SolTkOperation<SolTkSystem>("CreateSystemAsync");

            operation.Start();

            try
            {
                var entry = _context.Systems.Add(model);
                var saves = await _context.SaveChangesAsync();
                operation.Status.AddLogs($"Create Systems Saved: {saves} Entities.");
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

        public IQueryable<SolTkSystem> Query()
        {
            return _context.Systems;
        }

        public async Task<SolTkOperation<SolTkSystem>> UpdateAsync(SolTkSystem model)
        {
            var operation = new SolTkOperation<SolTkSystem>("UpdateSystemAsync");

            operation.Start();

            try
            {
                // Get Existing Entity:
                var getOperation = await GetAsync(model.Id);
                if (getOperation.ResultCode != SolTkOperationResultCode.Ok || getOperation.Data == null)
                {
                    operation.Status.AddErrors("The Model Provided for update wasn't found - Check Id/Data and submit again.");
                    operation.Stop();
                    return operation;
                }

                //operation.Operations.Add(getOperation);

                // Merge Entity and Model:
                getOperation.Data = getOperation.Data.Merge(model);
                var saves = await _context.SaveChangesAsync();

                operation.Status.AddLogs($"Systems Saved: {saves} Entities.");
                if (saves > 0) operation.ResultCode = SolTkOperationResultCode.Updated;
                if (saves == 0) operation.ResultCode = SolTkOperationResultCode.NoOp;

                operation.Data = getOperation.Data;
            }
            catch (Exception ex)
            {
                operation.Status.AddException(ex);
                operation.ResultCode = SolTkOperationResultCode.ExThrown;
            }

            operation.Stop();

            return operation;
        }


        public async Task<SolTkOperation<bool>> DeleteAsync(SolTkSystem model)
        {
            var operation = new SolTkOperation<bool>("CreateSystemAsync");

            operation.Start();

            try
            {
                _context.Systems.Remove(model);
                var saves = await _context.SaveChangesAsync();
                operation.Status.AddLogs($"Systems Saved: {saves} Entities.");
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