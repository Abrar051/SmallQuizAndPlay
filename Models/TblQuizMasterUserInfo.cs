using System;
using System.Collections.Generic;

namespace QuizMaster.Models
{
    public partial class TblQuizMasterUserInfo
    {
        public long Id { get; set; }
        public string Msisdn { get; set; }
        public string Name { get; set; }
        public string SourceUrl { get; set; }
        public string Ckey { get; set; }
        public DateTime? TimeStamp { get; set; }
        public string AppInAppToken { get; set; }
        public DateTime? AppInAppTokenTimeStamp { get; set; }
        public string ImageName { get; set; }
        public string PushNotificationToken { get; set; }
    }
}
