using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Models
{
    [Keyless]
    public class Leaderboard
    {
        public string Name { get; set; }
        public string MSISDN { get; set; }
        public long Rank { get; set; }
        public string TimeCount { get; set; }
        [Column(TypeName = "decimal(20,2)")]
        public decimal? Point { get; set; }
        public string Level { get; set; }
        [Column(TypeName = "decimal(20,2)")]
        public decimal? Score { get; set; }
        public string LevelStatus { get; set; }

    }
}
