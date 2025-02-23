using System.Linq.Expressions;

namespace TaskHub.Data.Repositories.Interfaces {
    public interface IRepository<T> where T : class {
        Task<T> AddAsync(T entity);
        Task<T> GetByIdAsync(Guid id);
        Task<T> UpdateAsync(Guid id, T entity);
        void DeleteAsync(Guid id);
        Task<T> FindAsync(Expression<Func<T, bool>> predicate);
    }
}