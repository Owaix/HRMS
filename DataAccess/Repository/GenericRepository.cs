using System;
using System.Linq;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Data;

namespace DataAccess.Repository
{
   public class GenericRepository<T> : IRepository<T> where T : BaseEntity
    {
        private DbContext entities = null;
        IDbSet<T> _objectSet;

        public GenericRepository(DbContext _entities)
        {
            entities = _entities;
            _objectSet = entities.Set<T>();
            // _objectSet = entities.CreateObjectSet<T>();
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate != null)
            {
                return _objectSet.Where(predicate);
            }

            return _objectSet.AsQueryable();
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {

            IQueryable<T> query = _objectSet.Where(predicate);
            return query;
        }

        public T Get(Expression<Func<T, bool>> predicate)
        {
            return _objectSet.First(predicate);
        }

        public void Add(T entity)
        {
            _objectSet.Add(entity);
        }

        public void Attach(T entity)
        {
            _objectSet.Attach(entity);
            entities.Entry<T>(entity).State = EntityState.Modified;

        }

        public void Delete(T entity)
        {
            _objectSet.Remove(entity);
        }
    }
}
