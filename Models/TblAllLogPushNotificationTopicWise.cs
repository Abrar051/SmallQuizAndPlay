using System;
using System.Collections.Generic;

namespace QuizMaster.Models
{
    public partial class TblAllLogPushNotificationTopicWise
    {
        public decimal Id { get; set; }
        public string Tokens { get; set; }
        public DateTime TimeStamp { get; set; }
        public string ServiceName { get; set; }
        public string Topic { get; set; }
        public int? Status { get; set; }
    }
}
