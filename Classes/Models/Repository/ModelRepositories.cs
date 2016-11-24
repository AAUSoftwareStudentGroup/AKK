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
            return _dbContext.Routes.Include(r => r.Section).Include(r => r.Member).Include(r => r.Grade).FirstOrDefault(d => d.Id == Id);
        }

        public override IEnumerable<Route> GetAll()
        {
            return _dbContext.Routes.Include(r => r.Section).Include(r => r.Member).Include(r => r.Grade).AsEnumerable();
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

    public class ImageRepository : DbSetRepository<Image>
    {
        private MainDbContext _dbContext;

        public ImageRepository(MainDbContext dbContext) : base(dbContext.Images, dbContext)
        {
            _dbContext = dbContext;
        }

        public override Image Find(Guid Id) {
            var image = _dbContext.Images.FirstOrDefault(d => d.Id == Id);
            image.Holds = _dbContext.Holds.Where(h => h.ImageId == Id).ToList();
            return image;
        }

        public override IEnumerable<Image> GetAll()
        {
            return _dbContext.Images;
        }
    }

    public class HoldRepository : DbSetRepository<Hold> 
    {
        public HoldRepository(MainDbContext dbContext) : base(dbContext.Holds, dbContext) 
        {
        }
    }

    public class MemberRepository : DbSetRepository<Member> {
        public MemberRepository(MainDbContext dbContext) : base(dbContext.Members, dbContext) {
        }
    }
}
