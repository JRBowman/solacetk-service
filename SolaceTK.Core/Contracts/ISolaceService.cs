using System;
using System.Linq;
using SolaceTK.Core.Models;
using SolaceTK.Core.Models.Files;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolaceTK.Core.Contracts
{
    public interface ISolaceService<T, TIndex>
    {
        // Sync Methods:
        public IEnumerable<T> Get();
        public T Get(TIndex index);
        public IQueryable<T> Query();
        public T Create(T model);
        public T Update(T model);
        public bool Delete(T model);
        public bool Validate(T model);
        public T Merge(T entity, T model);

        // Async Methods:
        public Task<IEnumerable<T>> GetAsync();
        public Task<T> GetAsync(TIndex index);
        public Task<T> CreateAsync(T model);
        public Task<T> UpdateAsync(T model);
        public Task<bool> DeleteAsync(T model);
        public Task<bool> ValidateAsync(T model);
        public Task<T> MergeAsync(T entity, T model);
        public Task<T> DiffAsync(T entity, T model);
        
        
    }
}