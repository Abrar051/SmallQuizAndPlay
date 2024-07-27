using System;
using System.Collections.Generic;

namespace QuizMaster.Models
{
    public partial class TblDailyLogin
    {
        public long Id { get; set; }
        public int? IsLogin { get; set; }
        public DateTime? Timestamp { get; set; }
        public string FbId { get; set; }
    }
}
