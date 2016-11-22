using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKK.Classes.Models.Repository
{
    public class TestRepository<TEntity> : IRepository<TEntity> where TEntity : class, IIdentifyable
    {
        private List<TEntity> _entities;

        public TestRepository() {
            _entities = new List<TEntity>();
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
