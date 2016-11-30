using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AKK.Models.Repositories
{
    public class RouteRepository : DbSetRepository<Route>
    {
        private readonly MainDbContext _dbContext;
        public RouteRepository(MainDbContext dbContext) : base(dbContext.Routes, dbContext)
        {
            _dbContext = dbContext;
        }
        public override Route Find(Guid Id)
        {
            return _dbContext.Routes.Include(r => r.Section).Include(r => r.Member).Include(r => r.Grade).Include(r => r.Videos).Include(r=>r.Ratings).Include(r=> r.Comments).Include(r => r.Image).FirstOrDefault(d => d.Id == Id);
        }

        public override IEnumerable<Route> GetAll()
        {
            return _dbContext.Routes.Include(r => r.Section).Include(r => r.Member).Include(r => r.Grade).Include(r => r.Videos).Include(r=> r.Ratings).Include(r=>r.Comments).Include(r => r.Image).AsEnumerable();
        }
    }
}