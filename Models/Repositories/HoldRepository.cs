namespace AKK.Models.Repositories
{
    public class HoldRepository:DbSetRepository<Hold> {
        public HoldRepository(MainDbContext dbContext) : base(dbContext.Holds, dbContext) {
        }
    }
}
