using LeaderBoardApi.Models;
using LeaderBoardApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LeaderBoardApi.QueryBuilder
{
    public class LeaderBoardGetAllQueryBuilder
    {
        private readonly LeaderBoardContext context;

        private string order = "ASC";
        private int limit = 10;
        private string user;

        public LeaderBoardGetAllQueryBuilder(LeaderBoardContext context)
        {
            this.context = context;
        }

        public LeaderBoardGetAllQueryBuilder OrderBy(string order = "ASC")
        {
            if(order == null) return this;

            this.order = order.ToUpper();
            return this;
        }

        public LeaderBoardGetAllQueryBuilder Limit(int limit)
        {
            if(limit == 0) return this;

            this.limit = limit;
            return this;
        }

        public LeaderBoardGetAllQueryBuilder User(string user)
        {
            this.user = user;
            return this;
        }


        public IQueryable<LeaderBoard> Query()
        {
            var query = context.LeaderBoards.AsQueryable();

            if(order != null)
            {
                if(order == "DESC") 
                    query = query.OrderByDescending(score => score.Score);
                else 
                    query = query.OrderBy(score => score.Score);
            }

            if(user != null)
                query = query.Where(score => score.User == user);

            query = query.Take(limit);

            return query;
        }
    }
}
