using System.Linq.Expressions;

namespace DepartmentEmployeeSystem.API.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, object>>[]? includes = null);
        Task<T?> GetByIdAsync(int id, Expression<Func<T, object>>[]? includes = null);
        Task<T?> FindAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[]? includes = null);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[]? includes = null);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
    }
}
