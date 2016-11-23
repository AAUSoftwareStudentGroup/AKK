using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace AKK.Classes.Models.Repository
{
    public class DbSetRepository<TEntity> : IRepository<TEntity> where TEntity : class, IIdentifyable
    {
        private MainDbContext _dbContext;
        private DbSet<TEntity> _dbset;
        public DbSetRepository(DbSet<TEntity> dbSet, MainDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbset = dbSet;
        }
        public virtual void Add(TEntity entity)
        {
            _dbset.Add(entity);
        }

        public virtual TEntity Find(Guid Id)
        {
           return _dbset.FirstOrDefault(d => d.Id == Id);
        }

        public virtual void Save()
        {
            _dbContext.SaveChanges();
        }

        public virtual void Delete(TEntity entity)
        {
            _dbset.Remove(Find(entity.Id));
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return _dbset.AsEnumerable();
        }
    }
}
