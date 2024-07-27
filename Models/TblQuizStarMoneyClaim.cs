using System;
using System.Collections.Generic;

namespace QuizMaster.Models
{
    public partial class TblQuizStarMoneyClaim
    {
        public long Id { get; set; }
        public string FbId { get; set; }
        public int? ClaimAmount { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}
