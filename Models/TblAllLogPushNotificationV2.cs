using System;
using System.Collections.Generic;

namespace QuizMaster.Models
{
    public partial class TblAllLogPushNotificationV2
    {
        public decimal Id { get; set; }
        public string SourceUrl { get; set; }
        public string Token { get; set; }
        public DateTime TimeStamp { get; set; }
        public string ServiceName { get; set; }
        public string Status { get; set; }
    }
}
