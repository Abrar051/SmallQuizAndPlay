using System;
using System.Collections.Generic;

namespace QuizMaster.Models.WapPortal
{
    public class TblJhotPotAnswerWCup
    {
        public long Id { get; set; }
        public long? JhotpotId { get; set; }
        public string Answer { get; set; }
        public string FbId { get; set; }
        public int? IsRight { get; set; }
        public DateTime? TimeStamp { get; set; }
        public int? TimeTaken { get; set; }
        public int? Round { get; set; }
        public string ServiceName { get; set; }
    }
}
