using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKK.Classes.Models.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Add(TEntity entity);
        TEntity Find(Guid Id);
        void Save();
        void Delete(TEntity entity);
        IEnumerable<TEntity> GetAll();
    }
}
