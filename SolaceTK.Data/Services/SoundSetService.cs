using SolaceTK.Data.Contexts;
using SolaceTK.Data;
using SolaceTK.Models.Sound;
using SolaceTK.Models.Telemetry;
using Microsoft.EntityFrameworkCore;
using SolaceTK.Models.Artifacts;

namespace SolaceTK.Data.Services
{
    public class SoundSetService : ISolaceService<SoundSet, int>
    {

        private SoundContext _context;

        public IQueryable<SoundSet> BaseQuery => _context.SoundSets;
        public IQueryable<SoundSet> AllQuery => _context.SoundSets.Include(x => x.Sources).ThenInclude(x => x.SoundData).Include(x => x.Sources);//.ThenInclude(x => x.Artifact);

        public SoundSetService(SoundContext context)
        {
            _context = context;
        }

        #region Async Methods

        public async Task<SolTkOperation<IEnumerable<SoundSet>>> GetAsync()
        {
            var operation = new SolTkOperation<IEnumerable<SoundSet>>("GetSoundSetsAsync");

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

        public async Task<SolTkOperation<SoundSet>> GetAsync(int index)
        {
            var operation = new SolTkOperation<SoundSet>("GetSoundSetAsync");

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

        public async Task<SolTkOperation<SoundSet>> GetAsync(string name)
        {
            var operation = new SolTkOperation<SoundSet>("GetSoundSetAsync");

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

        public async Task<SolTkOperation<SoundSet>> CreateAsync(SoundSet model)
        {
            var operation = new SolTkOperation<SoundSet>("CreateSoundSetAsync");

            operation.Start();

            try
            {
                var entry = _context.Add(model);
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

        public IQueryable<SoundSet> Query()
        {
            return _context.SoundSets;
        }

        public async Task<SolTkOperation<SoundSet>> UpdateAsync(SoundSet model)
        {
            var operation = new SolTkOperation<SoundSet>("UpdateSoundSetAsync");

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


        public async Task<SolTkOperation<bool>> DeleteAsync(SoundSet model)
        {
            var operation = new SolTkOperation<bool>("CreateSoundSetAsync");

            operation.Start();

            try
            {
                _context.Remove(model);
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