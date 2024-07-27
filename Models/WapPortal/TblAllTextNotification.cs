using System;
using System.Collections.Generic;

#nullable disable

namespace QuizMaster.Models.WapPortal
{
    public partial class TblAllTextNotification
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public string TextEngliash { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}
