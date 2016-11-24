namespace AKK.Models.Repositories
{
    public class GradeRepository:DbSetRepository<Grade> {
        public GradeRepository(MainDbContext dbContext) : base(dbContext.Grades, dbContext) {
        }
    }
}
