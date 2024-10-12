using MovieBest.BLL.SharedRepository;

namespace MovieBest.BLL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : class;
    }
}
