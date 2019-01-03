using System;
using System.Linq;
using System.Threading.Tasks;

namespace Telemetry.Database
{
    public interface IRepository<TEntity> where TEntity: class, IEntity
    {
        IQueryable<TEntity> GetAll();

        Task<TEntity> GetByIdAsync(Guid id);

        Task<bool> CreateAsync(TEntity entity);

        Task<bool> UpdateAsync(Guid id, TEntity entity);

        Task<bool> DeleteAsync(Guid id);
    }
}
