using System;
using System.Collections.Generic;

namespace QuizMaster.Models
{
    public partial class TblDailyQuizStartTime
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public string Hour { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? Timestamp { get; set; }
        public string CreatedBy { get; set; }
        public string C30min { get; set; }
        public string C15min { get; set; }
        public string C5min { get; set; }
        public string Minute { get; set; }
    }
}
