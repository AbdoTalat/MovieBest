using Microsoft.EntityFrameworkCore;
using MovieBest.DAL.DbContext;
using System.Linq.Expressions;

namespace MovieBest.BLL.SharedRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly MovieBestContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(MovieBestContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
            => await _dbSet.ToListAsync();
        public async Task<T?> GetByIdAsync(int Id)
            => await _dbSet.FindAsync(Id);
        public async Task AddNewAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> DeleteAsync(int Id)
        {
            var entity = await GetByIdAsync(Id);
            if (entity == null)
                return false;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;

        }


        public async Task<T?> GetSingleWithIncludeAsync(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IQueryable<T>> includeProperties)
        {
            IQueryable<T> query = _dbSet;

            query = includeProperties(query);

            return await query.FirstOrDefaultAsync(predicate);
        }
    }
}
