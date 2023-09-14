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
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SolaceTK.Data.Services
{
    public class AnimationService : ISolaceService<Animation, int>
    {

        private BehaviorContext _context;

        public IQueryable<Animation> BaseQuery => _context.Animations;
        public IQueryable<Animation> AllQuery => _context.Animations.Include(x => x.ActFrameData).ThenInclude(x => x.Frames)
                    .ThenInclude(x => x.DownstreamData).Include(x => x.Components).ThenInclude(x => x.ComponentData);

        public AnimationService(BehaviorContext context)
        {
            _context = context;
        }

        #region Async Methods

        public async Task<SolTkOperation<IEnumerable<Animation>>> GetAsync()
        {
            var operation = new SolTkOperation<IEnumerable<Animation>>("GetAnimationsAsync");

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

        public async Task<SolTkOperation<Animation>> GetAsync(int index)
        {
            var operation = new SolTkOperation<Animation>("GetAnimationAsync");

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

        public async Task<SolTkOperation<Animation>> GetAsync(string name)
        {
            var operation = new SolTkOperation<Animation>("GetAnimationAsync");

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

        public async Task<SolTkOperation<Animation>> CreateAsync(Animation model)
        {
            var operation = new SolTkOperation<Animation>("CreateAnimationAsync");

            operation.Start();

            try
            {
                var entry = _context.Animations.Add(model);
                var saves = await _context.SaveChangesAsync();
                operation.Status.AddLogs($"Animations Saved: {saves} Entities.");
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

        public IQueryable<Animation> Query()
        {
            return _context.Animations;
        }

        public async Task<SolTkOperation<Animation>> UpdateAsync(Animation model)
        {
            var operation = new SolTkOperation<Animation>("UpdateAnimationAsync");

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

                operation.Status.AddLogs($"Animations Saved: {saves} Entities.");
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


        public async Task<SolTkOperation<bool>> DeleteAsync(Animation model)
        {
            var operation = new SolTkOperation<bool>("CreateAnimationAsync");

            operation.Start();

            try
            {
                _context.Animations.Remove(model);
                var saves = await _context.SaveChangesAsync();
                operation.Status.AddLogs($"Animations Saved: {saves} Entities.");
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