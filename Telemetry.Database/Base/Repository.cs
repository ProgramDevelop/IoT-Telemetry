using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Telemetry.Database.Base
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity, new()
    {
        private readonly TelemetryContext _context;

        public Repository(TelemetryContext context)
        {
            _context = context;
        }

        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().AsNoTracking();
        }

        public TEntity GetById(Guid id)
        {
            var entity = _context.Set<TEntity>().AsNoTracking().FirstOrDefault(e => e.Id == id);
            return entity;
        }

        public bool Create(TEntity entity)
        {
            try
            {
                _context.Set<TEntity>().Add(entity);
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return false;
            }
            return true;
        }

        public bool Delete(Guid id)
        {
            var user = _context.Users.AsNoTracking().FirstOrDefault(u => u.Id == id);
            if (user == null)
                return false;
            try
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return false;
            }
            return true;
        }

        public bool Update(Guid id, TEntity entity)
        {
            try
            {
                _context.Set<TEntity>().Update(entity);
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return false;
            }
            return true;
        }
    }
}
