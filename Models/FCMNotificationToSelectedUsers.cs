using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Models
{
    public class FCMNotificationToSelectedUsers
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string ImageUrl { get; set; }
        public string RedirectUrl { get; set; }
        public string Service { get; set; }
        public string UserBase { get; set; }
        public string Campaign { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? ReScheduleTime { get; set; }  
        
    }
}
