using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AKK.Models.Repositories
{
    public class GradeRepository:DbSetRepository<Grade> {
        private readonly MainDbContext _dbContext;

        public GradeRepository(MainDbContext dbContext) : base(dbContext.Grades, dbContext)
        {
            _dbContext = dbContext;
        }

        public override Grade Find(Guid Id)
        {
            return _dbContext.Grades.Include(g => g.Routes)
                                    .FirstOrDefault(d => d.Id == Id);
        }

        public override IEnumerable<Grade> GetAll()
        {
            return _dbContext.Grades.Include(g => g.Routes)
                                    .AsEnumerable();
        }
        
    }
}
