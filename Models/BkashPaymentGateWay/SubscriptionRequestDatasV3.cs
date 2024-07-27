using System;
using System.Collections.Generic;

#nullable disable

namespace QuizMaster.Models.BkashPaymentGateWay
{
    public partial class SubscriptionRequestDatasV3
    {
        public decimal Id { get; set; }
        public string Accept { get; set; }
        public string Versions { get; set; }
        public string ChannelId { get; set; }
        public string XappKey { get; set; }
        public string ContentType { get; set; }
        public string Amount { get; set; }
        public string AmountQueryUrl { get; set; }
        public string FirstPaymentAmount { get; set; }
        public string FirstPaymentIncludedInCycle { get; set; }
        public string ServiceId { get; set; }
        public string Currency { get; set; }
        public string StartDate { get; set; }
        public string ExpiryDate { get; set; }
        public string Frequency { get; set; }
        public string SubscriptionType { get; set; }
        public string MaxCapAmount { get; set; }
        public string MaxCapRequired { get; set; }
        public string MerchantShortCode { get; set; }
        public string Payer { get; set; }
        public string PayerType { get; set; }
        public string PaymentType { get; set; }
        public string RedirectUrl { get; set; }
        public string SubscriptionRequestId { get; set; }
        public string SubscriptionReference { get; set; }
        public string ExtraParams { get; set; }
        public string ApiUrl { get; set; }
        public string Msisdn { get; set; }
        public string UserAgent { get; set; }
        public string Ip { get; set; }
        public string HsManufac { get; set; }
        public string HsMod { get; set; }
        public string HsDim { get; set; }
        public string HsOs { get; set; }
        public string ServiceName { get; set; }
        public string Ckey { get; set; }
        public string SourceUrl { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
