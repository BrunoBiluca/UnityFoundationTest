using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using LeaderBoardApi.Models;
using LeaderBoardApi.QueryBuilder;

namespace LeaderBoardApi.Controllers
{
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
        public async Task<ActionResult<IEnumerable<LeaderBoard>>> GetAll(
            [FromQuery(Name = "order")] string order,
            [FromQuery(Name = "limit")] int limit,
            CancellationToken cancellationToken = default
        )
        {
            return await new LeaderBoardGetAllQueryBuilder(context)
                .OrderBy(order)
                .Limit(limit)
                .Query()
                .ToListAsync(cancellationToken);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LeaderBoard>> Get(
            long id,
            CancellationToken cancellationToken = default
        )
        {
            var leaderBoard = await context.LeaderBoards.FindAsync(
                new object[] { id }, 
                cancellationToken
            );

            if (leaderBoard == null)
            {
                return NotFound();
            }

            return leaderBoard;
        }

        [HttpPost]
        public async Task<ActionResult<LeaderBoard>> Post(
            LeaderBoard leaderBoard,
            CancellationToken cancellationToken = default
        )
        {
            context.LeaderBoards.Add(leaderBoard);
            await context.SaveChangesAsync(cancellationToken);

            return CreatedAtAction(nameof(Get), new { id = leaderBoard.Id }, leaderBoard);
        }
    }
}
