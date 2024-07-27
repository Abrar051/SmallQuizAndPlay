using System;
using System.Collections.Generic;

#nullable disable

namespace QuizMaster.Models.BkashPaymentGateWay
{
    public partial class BkashSecret
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public decimal Amount { get; set; }
        public string FirstPaymentIncludedInCycle { get; set; }
        public string ServiceId { get; set; }
        public string Currency { get; set; }
        public string SubscriptionType { get; set; }
        public string MaxCapRequired { get; set; }
        public string MerchantShortCode { get; set; }
        public string PayerType { get; set; }
        public string PaymentType { get; set; }
        public string ServiceName { get; set; }
        public string AppNappUserName { get; set; }
        public string AppNappPassWord { get; set; }
    }
}
