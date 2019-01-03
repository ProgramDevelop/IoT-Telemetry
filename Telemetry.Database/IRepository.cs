using System.Linq;
using System.Threading.Tasks;

namespace Telemetry.Database
{
    public interface IRepository<TEntity> where TEntity: class
    {
        IQueryable<TEntity> GetAll();

        Task<TEntity> GetByIdAsync(int id);

        Task CreateAsync(TEntity entity);

        Task UpdateAsync(int id, TEntity entity);

        Task DeleteAsync(int id);
    }
}
