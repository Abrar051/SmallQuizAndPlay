using System;
using System.Collections.Generic;

#nullable disable

namespace QuizMaster.Models.BkashPaymentGateWay
{
    public partial class WebHookSubscriptionData
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string SubscriptionRequestId { get; set; }
        public string SubscriptionId { get; set; }
        public string SubscriptionStatus { get; set; }
        public string NextPaymentDate { get; set; }
        public string Amount { get; set; }
        public string TrxId { get; set; }
        public string TrxDate { get; set; }
        public string CancelledBy { get; set; }
        public string UpdateTime { get; set; }
        public string DueDate { get; set; }
        public string FirstPayment { get; set; }
        public string PaymentId { get; set; }
        public string PaymentStatus { get; set; }
        public string TimeStamp { get; set; }
        public string Type { get; set; }
    }
}
