using System;
using System.Collections.Generic;
using System.Linq;

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
            return _entities.FirstOrDefault(x => x.Id == Id);
        }

        public void Save()
        {
        }

        public void Delete(Guid id)
        {
            _entities.Remove(Find(id));
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _entities;
        }
    }
}
