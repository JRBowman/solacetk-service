using Microsoft.EntityFrameworkCore;
using SolaceTK.Data.Contexts;
using SolaceTK.Models.Artifacts;
using SolaceTK.Models.Telemetry;
using SolaceTK.Models.WorkItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaceTK.Data.Services
{
    public class CommentService : ISolaceService<WorkComment, int>
    {

        private WorkContext _context;

        public IQueryable<WorkComment> BaseQuery => _context.Comments;
        public IQueryable<WorkComment> AllQuery => _context.Comments.Include(x => x.Artifacts);

        public CommentService(WorkContext context)
        {
            _context = context;
        }

        #region Async Methods

        public async Task<SolTkOperation<IEnumerable<WorkComment>>> GetAsync()
        {
            var operation = new SolTkOperation<IEnumerable<WorkComment>>("GetWorkCommentsAsync");

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

        public async Task<SolTkOperation<WorkComment>> GetAsync(int index)
        {
            var operation = new SolTkOperation<WorkComment>("GetWorkCommentAsync");

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

        public async Task<SolTkOperation<WorkComment>> GetAsync(string name)
        {
            var operation = new SolTkOperation<WorkComment>("GetWorkCommentAsync");

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

        public async Task<SolTkOperation<WorkComment>> CreateAsync(WorkComment model)
        {
            var operation = new SolTkOperation<WorkComment>("CreateWorkCommentAsync");

            operation.Start();

            try
            {
                var entry = _context.Comments.Add(model);
                var saves = await _context.SaveChangesAsync();
                operation.Status.AddLogs($"WorkComments Saved: {saves} Entities.");
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

        public IQueryable<WorkComment> Query()
        {
            return _context.Comments;
        }

        public async Task<SolTkOperation<WorkComment>> UpdateAsync(WorkComment model)
        {
            var operation = new SolTkOperation<WorkComment>("UpdateWorkCommentAsync");

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


        public async Task<SolTkOperation<bool>> DeleteAsync(WorkComment model)
        {
            var operation = new SolTkOperation<bool>("CreateWorkCommentAsync");

            operation.Start();

            try
            {
                _context.Comments.Remove(model);
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
