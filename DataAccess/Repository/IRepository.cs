using System;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccess.Repository
{
    public interface IRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate = null);
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);        
        T Get(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Attach(T entity);
        void Delete(T entity);
    }
}
