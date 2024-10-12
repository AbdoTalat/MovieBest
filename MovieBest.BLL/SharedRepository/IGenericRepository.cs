using System.Linq.Expressions;

namespace MovieBest.BLL.SharedRepository
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<T?> GetByIdAsync(int Id);
        public Task AddNewAsync(T entity);
        public Task UpdateAsync(T entity);
        public Task<bool> DeleteAsync(int Id);
        public Task<T?> GetSingleWithIncludeAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, 
            IQueryable<T>> includeProperties);
    }
}
