using SolaceTK.Data.Contexts;
using SolaceTK.Models.Behavior;
using Microsoft.EntityFrameworkCore;
using SolaceTK.Models.Telemetry;
using SolaceTK.Models.Artifacts;

namespace SolaceTK.Data.Services
{
    public class StateService : ISolaceService<SolTkState, int>
    {

        private BehaviorContext _context;

        public IQueryable<SolTkState> BaseQuery => _context.States;
        public IQueryable<SolTkState> AllQuery => _context.States
                    .Include(x => x.Animation).ThenInclude(x => x.ActFrameData).ThenInclude(x => x.Frames).ThenInclude(x => x.DownstreamData)
                    .Include(x => x.Conditions)
                    .Include("NextStates")
                    .Include(x => x.Events).ThenInclude(x => x.Messages).ThenInclude(x => x.Data)
                    .Include(x => x.Events).ThenInclude(x => x.Conditions)
                    .Include(x => x.Events).ThenInclude(x => x.DownstreamData)
                    .Include("StartData").Include("EndData").Include("ActData");

        public StateService(BehaviorContext context)
        {
            _context = context;
        }

        public IQueryable<SolTkState> Query()
        {
            return _context.States;
        }

        #region Async Methods

        public async Task<SolTkOperation<IEnumerable<SolTkState>>> GetAsync()
        {
            var operation = new SolTkOperation<IEnumerable<SolTkState>>("GetStatesAsync");

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

        public async Task<SolTkOperation<SolTkState>> GetAsync(int index)
        {
            var operation = new SolTkOperation<SolTkState>("GetStateAsync");

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

        public async Task<SolTkOperation<SolTkState>> GetAsync(string name)
        {
            var operation = new SolTkOperation<SolTkState>("GetStateAsync");

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

        public async Task<SolTkOperation<SolTkState>> CreateAsync(SolTkState model)
        {
            var operation = new SolTkOperation<SolTkState>("CreateStateAsync");

            operation.Start();

            try
            {
                var entry = _context.States.Add(model);
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

        public async Task<SolTkOperation<SolTkState>> UpdateAsync(SolTkState model)
        {
            var operation = new SolTkOperation<SolTkState>("UpdateStateAsync");

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

                operation.Status.AddLogs($"States Saved: {saves} Entities.");
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

        public async Task<SolTkOperation<bool>> DeleteAsync(SolTkState model)
        {
            var operation = new SolTkOperation<bool>("DeleteStateAsync");

            operation.Start();

            try
            {
                _context.States.Remove(model);
                var saves = await _context.SaveChangesAsync();
                operation.Status.AddLogs($"States Saved: {saves} Entities.");
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