using SolaceTK.Data.Contexts;
using SolaceTK.Data;
using SolaceTK.Models.WorkItems;
using SolaceTK.Models.Telemetry;
using Microsoft.EntityFrameworkCore;
using SolaceTK.Models.Artifacts;

namespace SolaceTK.Data.Services
{
    public class ProjectService : ISolaceService<WorkProject, int>
    {
        private WorkContext _context;

        public IQueryable<WorkProject> BaseQuery => _context.Projects;
        public IQueryable<WorkProject> AllQuery => _context.Projects.Include(x => x.WorkItems).ThenInclude(x => x.Comments).Include(x => x.Comments).Include(x => x.Payments);

        public ProjectService(WorkContext context)
        {
            _context = context;
        }

        #region Async Methods

        public async Task<SolTkOperation<IEnumerable<WorkProject>>> GetAsync()
        {
            var operation = new SolTkOperation<IEnumerable<WorkProject>>("GetProjectsAsync");

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

        public async Task<SolTkOperation<WorkProject>> GetAsync(int index)
        {
            var operation = new SolTkOperation<WorkProject>("GetProjectAsync");

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

        public async Task<SolTkOperation<WorkProject>> GetAsync(string name)
        {
            var operation = new SolTkOperation<WorkProject>("GetProjectAsync");

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

        public async Task<SolTkOperation<WorkProject>> CreateAsync(WorkProject model)
        {
            var operation = new SolTkOperation<WorkProject>("CreateProjectAsync");

            operation.Start();

            try
            {
                var entry = _context.Projects.Add(model);
                var saves = await _context.SaveChangesAsync();
                operation.Status.AddLogs($"Projects Saved: {saves} Entities.");
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

        public IQueryable<WorkProject> Query()
        {
            return _context.Projects;
        }

        public async Task<SolTkOperation<WorkProject>> UpdateAsync(WorkProject model)
        {
            var operation = new SolTkOperation<WorkProject>("UpdateProjectAsync");

            operation.Start();

            try
            {
                // Get Existing Entity:
                var getOperation = await GetAsync(model.Id);
                if (getOperation.ResultCode != SolTkOperationResultCode.Ok)
                {
                    operation.Status.AddErrors("The Model Provided for update wasn't found - Check Id/Data and submit again.");
                    operation.Stop();
                    return operation;
                }

                // Merge Entity and Model:
                //_context.Entry(getOperation.Data).CurrentValues.SetValues(model);
                getOperation.Data.Merge(model);
                var saves = await _context.SaveChangesAsync();

                operation.Status.AddLogs($"Projects Saved: {saves} Entities.");
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

        public async Task<SolTkOperation<bool>> DeleteAsync(WorkProject model)
        {
            var operation = new SolTkOperation<bool>("CreateProjectAsync");

            operation.Start();

            try
            {
                _context.Projects.Remove(model);
                var saves = await _context.SaveChangesAsync();
                operation.Status.AddLogs($"Projects Saved: {saves} Entities.");
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