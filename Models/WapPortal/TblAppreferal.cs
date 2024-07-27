using System;
using System.Collections.Generic;

#nullable disable

namespace QuizMaster.Models.WapPortal
{
    public partial class TblAppreferal
    {
        public long Id { get; set; }
        public string InviteFbid { get; set; }
        public string UserFbId { get; set; }
        public string Type { get; set; }
        public string App { get; set; }
        public int? IsSync { get; set; }
        public int? Coin { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}
