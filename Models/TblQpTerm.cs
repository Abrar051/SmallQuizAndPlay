using System;
using System.Collections.Generic;

namespace QuizMaster.Models
{
    public partial class TblQpTerm
    {
        public long Id { get; set; }
        public int? Serial { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public string MessageEnglish { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}
