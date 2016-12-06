namespace AKK.Models.Repositories
{
    public class HoldColorRepository: DbSetRepository<HoldColor>
    {
        public HoldColorRepository(MainDbContext dbContext) : base(dbContext.HoldColors, dbContext)
        {
            
        }
    }
}
