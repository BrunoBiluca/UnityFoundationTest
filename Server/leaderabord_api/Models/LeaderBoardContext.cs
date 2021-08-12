using Microsoft.EntityFrameworkCore;

namespace LeaderBoardApi.Models
{
    public class LeaderBoardContext : DbContext
    {
        public LeaderBoardContext(DbContextOptions<LeaderBoardContext> options) : base(options) { }

        public DbSet<LeaderBoard> LeaderBoards { get; set; }

    }
}