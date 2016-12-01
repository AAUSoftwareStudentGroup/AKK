using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace AKK.Models.Repositories
{
    public class MemberRepository:DbSetRepository<Member>
    {
        private readonly MainDbContext _dbContext;

        public MemberRepository(MainDbContext dbContext) : base(dbContext.Members, dbContext)
        {
            _dbContext = dbContext;
        }

        public override Member Find(Guid Id)
        {
            return _dbContext.Members.Include(r => r.Ratings)
                                        .ThenInclude(r => r.Member)
                                    .FirstOrDefault(d => d.Id == Id);
        }

        public override IEnumerable<Member> GetAll()
        {
            return _dbContext.Members.Include(r => r.Ratings)
                                        .ThenInclude(r => r.Member)
                                    .AsEnumerable();
        }
    }
}
