using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace DataAccess.UOW
{
    public class GenericUnitOfWork : IDisposable, IGenericUnitOfWork
    {
        private DbContext entities = null;


        public DbContext Context { get { return entities; } }

        //public GenericUnitOfWork()
        //{
        //    entities = new QMSContext();
        //}

        public GenericUnitOfWork(QMSContext _qmsContext)
        {
            entities = _qmsContext;
        }

        public Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public IRepository<T> Repository<T>() where T : BaseEntity
        {
            if (repositories.Keys.Contains(typeof(T)) == true)
            {
                return repositories[typeof(T)] as IRepository<T>;
            }
            IRepository<T> repo = new GenericRepository<T>(entities);
            repositories.Add(typeof(T), repo);
            return repo;
        }

        public void SaveChanges()
        {
            entities.SaveChanges();
        }

        //public TElement ExecuteSql<TElement>(string sql, params object[] parameters) where TElement : class
        //{
        //    var result = this.entities.Database.SqlQuery<TElement>(sql, parameters) as TElement;
        //    return result ;
        //}


        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    entities.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
