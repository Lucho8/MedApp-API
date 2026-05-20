using System.Linq.Expressions;

namespace MedApp.Domain.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    void Update(T entity);
    void Remove(T entity);
    Task<IEnumerable<T>> GetAllWithIncludesAsync(params System.Linq.Expressions.Expression<Func<T, object>>[] includes);
    Task<IEnumerable<T>> QueryAsync(Func<IQueryable<T>, IQueryable<T>> queryBuilder);

}