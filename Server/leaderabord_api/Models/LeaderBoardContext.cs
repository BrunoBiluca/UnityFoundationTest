using LeaderBoardApi.Models;
using Microsoft.EntityFrameworkCore;

public class LeaderBoardContext : DbContext {
    public LeaderBoardContext(DbContextOptions<LeaderBoardContext> options) : base(options) { }

    public DbSet<LeaderBoard> LeaderBoards { get; set; }

}