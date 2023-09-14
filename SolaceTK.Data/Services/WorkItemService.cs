using System;
using System.Linq;
using SolaceTK.Models.Behavior;
using SolaceTK.Models.Artifacts;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SolaceTK.Data.Contexts;
using SolaceTK.Data;
using SolaceTK.Models.Telemetry;
using SolaceTK.Models.WorkItems;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SolaceTK.Data.Services
{
    public class WorkItemService : ISolaceService<WorkItem, int>
    {

        private WorkContext _context;

        public IQueryable<WorkItem> BaseQuery => _context.WorkItems;
        public IQueryable<WorkItem> AllQuery => _context.WorkItems.Include(x => x.Artifacts).Include(x => x.Comments).Include(x => x.Payment);

        public WorkItemService(WorkContext context)
        {
            _context = context;
        }

        #region Async Methods

        public async Task<SolTkOperation<IEnumerable<WorkItem>>> GetAsync()
        {
            var operation = new SolTkOperation<IEnumerable<WorkItem>>("GetWorkItemsAsync");

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

        public async Task<SolTkOperation<WorkItem>> GetAsync(int index)
        {
            var operation = new SolTkOperation<WorkItem>("GetWorkItemAsync");

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

        public async Task<SolTkOperation<WorkItem>> GetAsync(string name)
        {
            var operation = new SolTkOperation<WorkItem>("GetWorkItemAsync");

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

        public async Task<SolTkOperation<WorkItem>> CreateAsync(WorkItem model)
        {
            var operation = new SolTkOperation<WorkItem>("CreateWorkItemAsync");

            operation.Start();

            try
            {
                var entry = _context.WorkItems.Add(model);
                var saves = await _context.SaveChangesAsync();
                operation.Status.AddLogs($"WorkItems Saved: {saves} Entities.");
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

        public IQueryable<WorkItem> Query()
        {
            return _context.WorkItems;
        }

        public async Task<SolTkOperation<WorkItem>> UpdateAsync(WorkItem model)
        {
            var operation = new SolTkOperation<WorkItem>("UpdateWorkItemAsync");

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

                operation.Status.AddLogs($"Work Items Saved: {saves} Entities.");
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


        public async Task<SolTkOperation<bool>> DeleteAsync(WorkItem model)
        {
            var operation = new SolTkOperation<bool>("CreateWorkItemAsync");

            operation.Start();

            try
            {
                _context.WorkItems.Remove(model);
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