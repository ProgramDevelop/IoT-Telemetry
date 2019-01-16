using System;
using System.Linq;

namespace Telemetry.Database.Base
{
    public interface IRepository<TEntity> where TEntity: class, IEntity, new()
    {
        IQueryable<TEntity> GetAll();

        TEntity GetById(Guid id);

        bool Create(TEntity entity);

        bool Update(Guid id, TEntity entity);

        bool Delete(Guid id);
    }
}
