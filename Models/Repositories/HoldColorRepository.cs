using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKK.Models.Repositories
{
    public class HoldColorRepository: DbSetRepository<HoldColor>
    {
        public HoldColorRepository(MainDbContext dbContext) : base(dbContext.HoldColors, dbContext)
        {
            
        }
    }
}
