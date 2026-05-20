using System.Linq.Expressions;
using MedApp.Domain.Interfaces;
using MedApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace MedApp.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(Guid id) =>
        await _dbSet.FindAsync(id);

    public async Task<IEnumerable<T>> GetAllAsync() =>
        await _dbSet.ToListAsync();

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) =>
        await _dbSet.Where(predicate).ToListAsync();

    public async Task AddAsync(T entity) =>
        await _dbSet.AddAsync(entity);

    public void Update(T entity) =>
        _dbSet.Update(entity);

    public void Remove(T entity) =>
        _dbSet.Remove(entity);

    public async Task<IEnumerable<T>> GetAllWithIncludesAsync(
    params Expression<Func<T, object>>[] includes)
    {
    IQueryable<T> query = _dbSet;
    foreach (var include in includes)
        query = query.Include(include);
    return await query.ToListAsync();
    }

    public async Task<IEnumerable<T>> QueryAsync(Func<IQueryable<T>, IQueryable<T>> queryBuilder)
    {
        IQueryable<T> query = _dbSet;
        query = queryBuilder(query);
        return await query.ToListAsync();
    }
}