using System;
using System.Collections.Generic;

namespace QuizMaster.Models
{
    public partial class TblDailyAppResponse
    {
        public long Id { get; set; }
        public string Fbid { get; set; }
        public string Msisdn { get; set; }
        public string Response { get; set; }
        public long? Qid { get; set; }
        public int? Timetaken { get; set; }
        public DateTime? Timestamp { get; set; }
        public int? Level { get; set; }
        public string Type { get; set; }
    }
}
