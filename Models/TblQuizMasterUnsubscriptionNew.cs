using System;
using System.Collections.Generic;

namespace QuizMaster.Models
{
    public partial class TblQuizMasterUnsubscriptionNew
    {
        public long Id { get; set; }
        public string ServiceName { get; set; }
        public string UnsubscriptionNumber { get; set; }
        public int? Answer { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}
