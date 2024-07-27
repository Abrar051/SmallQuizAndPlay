using System;
using System.Collections.Generic;

#nullable disable

namespace QuizMaster.Models.WapPortal
{
    public partial class TblRewardCoinActivity
    {
        public long Id { get; set; }
        public string Fbid { get; set; }
        public int? CoinReward { get; set; }
        public string Type { get; set; }
        public int? IsSync { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}
