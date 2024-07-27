using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Models
{
    public class UnSubscriptionResponseModel
    {
        public int Id { get; set; }
        public string SubscriptionId { get; set; }
        public string SubscriptionStatus { get; set; }
        public string Message { get; set; }
        public string Instant { get; set; }
    }
}
