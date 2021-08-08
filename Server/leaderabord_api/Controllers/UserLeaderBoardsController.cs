using LeaderBoardApi.Models;
using LeaderBoardApi.QueryBuilder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeaderBoardApi.Controllers
{
    [Route("api/users/{user}/leaderboards")]
    [ApiController]
    public class UserLeaderBoardsController
    {
        private readonly LeaderBoardContext context;

        public UserLeaderBoardsController(LeaderBoardContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeaderBoard>>> GetAll(
            [FromRoute(Name = "user")] string user,
            [FromQuery(Name = "order")] string order = "ASC",
            [FromQuery(Name = "limit")] int limit = 10,
            CancellationToken cancellationToken = default
        )
        {
            return await new LeaderBoardGetAllQueryBuilder(context)
                .User(user)
                .OrderBy(order)
                .Limit(limit)
                .Query()
                .ToListAsync(cancellationToken);
        }
    }
}
