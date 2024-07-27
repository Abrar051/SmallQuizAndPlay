using System;
using System.Collections.Generic;

namespace QuizMaster.Models
{
    public partial class TblDailyPlayStatus
    {
        public long Id { get; set; }
        public string FbId { get; set; }
        public string Type { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}
