using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using DepartmentEmployeeSystem.API.Data;
using DepartmentEmployeeSystem.API.Interfaces;

namespace DepartmentEmployeeSystem.API.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, object>>[]? includes = null)
        {
            IQueryable<T> query = _dbSet;
            
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            return await query.ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(int id, Expression<Func<T, object>>[]? includes = null)
        {
            IQueryable<T> query = _dbSet;
            
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, GetPrimaryKeyName()) == id);
        }

        public virtual async Task<T?> FindAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[]? includes = null)
        {
            IQueryable<T> query = _dbSet;
            
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            return await query.FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[]? includes = null)
        {
            IQueryable<T> query = _dbSet;
            
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            return await query.Where(predicate).ToListAsync();
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return false;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        {
            return predicate == null ? await _dbSet.CountAsync() : await _dbSet.CountAsync(predicate);
        }

        private string GetPrimaryKeyName()
        {
            var entityType = _context.Model.FindEntityType(typeof(T));
            var primaryKey = entityType?.FindPrimaryKey();
            return primaryKey?.Properties[0].Name ?? "Id";
        }
    }
}
