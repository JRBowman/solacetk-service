using SolaceTK.Models.Behavior;
using SolaceTK.Models.Telemetry;

namespace SolaceTK.Data
{
    public interface ISolaceService<T, TIndex>
    {
        // Sync Methods:
        //public SolTkOperation<IEnumerable<T>> Get();
        //public SolTkOperation<T> Get(TIndex index);
        //public IQueryable<T> Query();
        //public SolTkOperation<T> Create(T model);
        //public SolTkOperation<T> Update(T model);
        //public SolTkOperation<bool> Delete(T model);
        //public SolTkOperation<bool> Validate(T model);
        public IQueryable<T> BaseQuery { get; }
        public IQueryable<T> AllQuery { get; }

        // Async Methods:
        public Task<SolTkOperation<IEnumerable<T>>> GetAsync();
        public Task<SolTkOperation<T>> GetAsync(TIndex index);
        //public Task<SolTkOperation<T>> GetAsync(string name);
        public Task<SolTkOperation<T>> CreateAsync(T model);
        public Task<SolTkOperation<T>> UpdateAsync(T model);
        public Task<SolTkOperation<bool>> DeleteAsync(T model);
        //public Task<SolTkOperation<bool>> ValidateAsync(T model);

    }
}