using System;
using System.Collections.Generic;

namespace QuizMaster.Models
{
    public partial class TblAllLogPushNotificationMessageSending
    {
        public decimal Id { get; set; }
        public string Tokens { get; set; }
        public DateTime TimeStamp { get; set; }
        public string ServiceName { get; set; }
        public string Topic { get; set; }
        public string NotificationTitle { get; set; }
        public string NotificationBody { get; set; }
        public string Campaign { get; set; }
        public string UserBase { get; set; }
        public DateTime? ReScheduleTime { get; set; }
        public int? Status { get; set; }
        public string RedirectUrl { get; set; }
    }
}
