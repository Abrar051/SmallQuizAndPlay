using System;
using System.Collections.Generic;

namespace QuizMaster.Models
{
    public partial class TblQuizMasterWordPuzzleGame
    {
        public decimal Id { get; set; }
        public string Msisdn { get; set; }
        public string GameName { get; set; }
        public DateTime? TimeStamp { get; set; }
        public int? Score { get; set; }
        public decimal? TimeCount { get; set; }
    }
}
