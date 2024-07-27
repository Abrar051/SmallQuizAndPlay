using System;
using System.Collections.Generic;

#nullable disable

namespace QuizMaster.Models.WapPortal
{
    public partial class TblJhotpotPlayCount
    {
        public long Id { get; set; }
        public string FbId { get; set; }
        public int? Count { get; set; }
        public DateTime? TimeStamp { get; set; }
        public string ServiceName { get; set; }
    }
}
