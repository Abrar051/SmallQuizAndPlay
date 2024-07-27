using System;
using System.Collections.Generic;

namespace QuizMaster.Models
{
    public partial class TblLikeViewPortal
    {
        public long Id { get; set; }
        public string Msisdn { get; set; }
        public string Contentcode { get; set; }
        public string Portal { get; set; }
        public DateTime? Timestamp { get; set; }
        public long? Count { get; set; }
    }
}
