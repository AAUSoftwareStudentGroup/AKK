namespace AKK.Models.Repositories
{
    public class MemberRepository : DbSetRepository<Member> 
    {
        public MemberRepository(MainDbContext dbContext) : base(dbContext.Members, dbContext) 
        {
        }
    }
}
