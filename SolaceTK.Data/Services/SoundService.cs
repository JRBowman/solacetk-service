using SolaceTK.Data.Contexts;
using SolaceTK.Data;
using SolaceTK.Models.Sound;
using SolaceTK.Models.Telemetry;
using Microsoft.EntityFrameworkCore;

namespace SolaceTK.Data.Services
{
    public class SoundService : ISolaceService<SoundSource, int>
    {

        private SoundContext _context;

        public IQueryable<SoundSource> BaseQuery => _context.SoundSources;
        public IQueryable<SoundSource> AllQuery => _context.SoundSources.Include(x => x.SoundData);//.Include(x => x.Artifact);

        public SoundService(SoundContext context)
        {
            _context = context;
        }

        #region Async Methods

        public async Task<SolTkOperation<IEnumerable<SoundSource>>> GetAsync()
        {
            var operation = new SolTkOperation<IEnumerable<SoundSource>>("GetSoundSourcesAsync");

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

        public async Task<SolTkOperation<SoundSource>> GetAsync(int index)
        {
            var operation = new SolTkOperation<SoundSource>("GetSoundSourceAsync");

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

        public async Task<SolTkOperation<SoundSource>> GetAsync(string name)
        {
            var operation = new SolTkOperation<SoundSource>("GetSoundSourceAsync");

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

        public async Task<SolTkOperation<SoundSource>> CreateAsync(SoundSource model)
        {
            var operation = new SolTkOperation<SoundSource>("CreateSoundSourceAsync");

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

        public IQueryable<SoundSource> Query()
        {
            return _context.SoundSources;
        }

        public async Task<SolTkOperation<SoundSource>> UpdateAsync(SoundSource model)
        {
            var operation = new SolTkOperation<SoundSource>("UpdateSoundSourceAsync");

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


        public async Task<SolTkOperation<bool>> DeleteAsync(SoundSource model)
        {
            var operation = new SolTkOperation<bool>("CreateSoundSourceAsync");

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