using System;
using System.Collections.Generic;

namespace QuizMaster.Models
{
    public partial class TblFreePlayStatus
    {
        public long Id { get; set; }
        public string Fbid { get; set; }
        public int? IsPlay { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}
