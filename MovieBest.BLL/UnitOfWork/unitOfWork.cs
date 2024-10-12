using MovieBest.BLL.SharedRepository;
using MovieBest.DAL.DbContext;

namespace MovieBest.BLL.UnitOfWork
{
    public class unitOfWork : IUnitOfWork
    {
        private readonly MovieBestContext _context;
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public unitOfWork(MovieBestContext context)
        {
            _context = context;
        }
        public void Dispose() { }

        public IGenericRepository<T> Repository<T>() where T : class
        {
            if (_repositories.ContainsKey(typeof(T)))
            {
                return _repositories[typeof(T)] as IGenericRepository<T>;
            }
            var repository = new GenericRepository<T>(_context);
            _repositories[typeof(T)] = repository;
            return repository;
        }
    }
}
