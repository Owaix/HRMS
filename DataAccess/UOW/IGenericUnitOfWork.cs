using DataAccess.Repository;
using System.Data.Entity;

namespace DataAccess.UOW
{
    public interface IGenericUnitOfWork
    {
        DbContext Context { get; }

        void Dispose();
        IRepository<T> Repository<T>() where T : BaseEntity;
        void SaveChanges();
    }
}