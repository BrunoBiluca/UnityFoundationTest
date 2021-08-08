using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LeaderBoardApi.Models
{
    public class LeaderBoard
    {
        public Guid Id { get; set; }
        public long Score { get; set; }
        public string User { get; set; }
    }
}