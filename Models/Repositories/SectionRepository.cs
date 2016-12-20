namespace AKK.Models.Repositories
{
    public class SectionRepository:DbSetRepository<Section> {
        public SectionRepository(MainDbContext dbContext) : base(dbContext.Sections, dbContext) {
        }
    }
}
