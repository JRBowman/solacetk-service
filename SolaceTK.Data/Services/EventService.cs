using SolaceTK.Data.Contexts;
using SolaceTK.Data;
using SolaceTK.Models.Events;
using SolaceTK.Models.Telemetry;
using Microsoft.EntityFrameworkCore;
using SolaceTK.Models.Artifacts;

namespace SolaceTK.Data.Services
{
    public class EventService : ISolaceService<SolTkEvent, int>
    {

        private BehaviorContext _context;

        public IQueryable<SolTkEvent> BaseQuery => _context.Events;
        public IQueryable<SolTkEvent> AllQuery => _context.Events.Include("Conditions").Include("DownstreamData")
                 .Include(x => x.Messages).ThenInclude(x => x.Data);

        public EventService(BehaviorContext context)
        {
            _context = context;
        }

        #region Async Methods

        public async Task<SolTkOperation<IEnumerable<SolTkEvent>>> GetAsync()
        {
            var operation = new SolTkOperation<IEnumerable<SolTkEvent>>("GetEventsAsync");

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

        public async Task<SolTkOperation<SolTkEvent>> GetAsync(int index)
        {
            var operation = new SolTkOperation<SolTkEvent>("GetEventAsync");

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

        public async Task<SolTkOperation<SolTkEvent>> GetAsync(string name)
        {
            var operation = new SolTkOperation<SolTkEvent>("GetEventAsync");

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

        public async Task<SolTkOperation<SolTkEvent>> CreateAsync(SolTkEvent model)
        {
            var operation = new SolTkOperation<SolTkEvent>("CreateEventAsync");

            operation.Start();

            try
            {
                var entry = _context.Events.Add(model);
                var saves = await _context.SaveChangesAsync();
                operation.Status.AddLogs($"Events Saved: {saves} Entities.");
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

        public IQueryable<SolTkEvent> Query()
        {
            return _context.Events;
        }

        public async Task<SolTkOperation<SolTkEvent>> UpdateAsync(SolTkEvent model)
        {
            var operation = new SolTkOperation<SolTkEvent>("UpdateEventAsync");

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

                operation.Status.AddLogs($"Events Saved: {saves} Entities.");
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


        public async Task<SolTkOperation<bool>> DeleteAsync(SolTkEvent model)
        {
            var operation = new SolTkOperation<bool>("CreateEventAsync");

            operation.Start();

            try
            {
                _context.Events.Remove(model);
                var saves = await _context.SaveChangesAsync();
                operation.Status.AddLogs($"Events Saved: {saves} Entities.");
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