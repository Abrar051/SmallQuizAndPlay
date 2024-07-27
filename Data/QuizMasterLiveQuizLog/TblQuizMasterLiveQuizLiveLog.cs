using System;
using System.Collections.Generic;

#nullable disable

namespace QuizMaster.Data.QuizMasterLiveQuizLog
{
    public partial class TblQuizMasterLiveQuizLiveLog
    {
        public long Id { get; set; }
        public string Msisdn { get; set; }
        public string ThemId { get; set; }
        public string Ckey { get; set; }
        public string SourceUrl { get; set; }
        public DateTime? TimeStamp { get; set; }
        public string ExtraPram1 { get; set; }
        public string ExtraPram2 { get; set; }
        public string ExtraPram3 { get; set; }
    }
}
