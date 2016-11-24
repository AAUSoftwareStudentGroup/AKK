using System;
using System.Collections.Generic;

namespace AKK.Models.Repositories
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
