using System;
using System.Collections.Generic;

namespace AKK.Models.Repositories
{
    public class TestRepository<TEntity> : IRepository<TEntity> where TEntity : class, IIdentifyable
    {
        private List<TEntity> _entities;

        public TestRepository() {
            _entities = new List<TEntity>();
        }

        public TestRepository(List<TEntity> entities) {
            _entities = entities;
        }

        public void Add(TEntity entity)
        {
            _entities.Add(entity);
        }

        public TEntity Find(Guid Id)
        {
            return _entities.Find(x => x.Id == Id);
        }

        public void Save()
        {
        }

        public void Delete(TEntity entity)
        {
            _entities.Remove(Find(entity.Id));
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _entities;
        }
    }
}
