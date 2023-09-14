using SolaceTK.Data.Contexts;
using SolaceTK.Data;
using SolaceTK.Models.Interfaces;
using SolaceTK.Models.Telemetry;
using SolaceTK.Models.Artifacts;

namespace SolaceTK.Data.Services
{
    public class InterfaceService : ISolaceService<Hud, int>
    {

        private EnvironmentContext _context;

        public IQueryable<Hud> BaseQuery => null;
        public IQueryable<Hud> AllQuery => BaseQuery;

        public InterfaceService(EnvironmentContext context)
        {
            _context = context;
        }

        #region Async Methods

        public async Task<SolTkOperation<IEnumerable<Hud>>> GetAsync()
        {
            var operation = new SolTkOperation<IEnumerable<Hud>>("GetHudsAsync");

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

        public async Task<SolTkOperation<Hud>> GetAsync(int index)
        {
            var operation = new SolTkOperation<Hud>("GetHudAsync");

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

        public async Task<SolTkOperation<Hud>> GetAsync(string name)
        {
            var operation = new SolTkOperation<Hud>("GetHudAsync");

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

        public async Task<SolTkOperation<Hud>> CreateAsync(Hud model)
        {
            var operation = new SolTkOperation<Hud>("CreateHudAsync");

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

        public IQueryable<Hud> Query()
        {
            return null;
        }

        public async Task<SolTkOperation<Hud>> UpdateAsync(Hud model)
        {
            var operation = new SolTkOperation<Hud>("UpdateHudAsync");

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


        public async Task<SolTkOperation<bool>> DeleteAsync(Hud model)
        {
            var operation = new SolTkOperation<bool>("CreateHudAsync");

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

        #endregion
    }
}