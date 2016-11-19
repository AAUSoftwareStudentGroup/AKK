using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace AKK.Classes.Models.Repository
{
    public class RouteRepository : DbSetRepository<Route>
    {
        private MainDbContext _dbContext;
        public RouteRepository(MainDbContext dbContext) : base(dbContext.Routes, dbContext)
        {
            _dbContext = dbContext;
        }
        public override Route Find(Guid Id)
        {
            return _dbContext.Routes.Include(r => r.Section).Include(r => r.Grade).FirstOrDefault(d => d.Id == Id);
        }

        public override IEnumerable<Route> GetAll()
        {
            return _dbContext.Routes.Include(r => r.Section).Include(r => r.Grade).AsEnumerable();
        }
    }

    public class SectionRepository : DbSetRepository<Section>
    {
        public SectionRepository(MainDbContext dbContext) : base(dbContext.Sections, dbContext)
        {
            
        }
    }

    public class GradeRepository : DbSetRepository<Grade>
    {
        public GradeRepository(MainDbContext dbContext) : base(dbContext.Grades, dbContext)
        {
            
        }
    }
}
