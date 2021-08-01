using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace leaderabord_api.Controllers {
    [Route("api/leaderboards")]
    [ApiController]
    public class LeaderBoardsController : ControllerBase
    {
        private readonly LeaderBoardContext context;

        public LeaderBoardsController(LeaderBoardContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeaderBoard>>> GetAll()
        {
            return await context.LeaderBoards.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LeaderBoard>> Get(long id)
        {
            var leaderBoard = await context.LeaderBoards.FindAsync(id);

            if (leaderBoard == null)
            {
                return NotFound();
            }

            return leaderBoard;
        }

        [HttpPost]
        public async Task<ActionResult<LeaderBoard>> Post(LeaderBoard leaderBoard)
        {
            context.LeaderBoards.Add(leaderBoard);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = leaderBoard.Id }, leaderBoard);
        }
    }
}
