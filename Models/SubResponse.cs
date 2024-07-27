using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Models
{
    public class SubResponse
    {
        public int Id { get; set; }
        public string SubscriptionRequestId { get; set; }
        public string RedirectURL { get; set; }
        public string ExpirationTime { get; set; }
        public string TimeStamp { get; set; }
        public string Status { get; set; }
    }
}
