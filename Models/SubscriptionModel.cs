namespace QuizMaster.Models
{
    public class SubscriptionModel
    {
        public string amount { get; set; }
        public string firstPaymentIncludedInCycle { get; set; }
        public string serviceId { get; set; }
        public string currency { get; set; }
        public string startDate { get; set; }
        public string expiryDate { get; set; }
        public string frequency { get; set; }
        public string subscriptionType { get; set; }
        public string maxCapRequired { get; set; }
        public string merchantShortCode { get; set; }
        public string payerType { get; set; }
        public string paymentType { get; set; }
        public string redirectUrl { get; set; }
        public string subscriptionRequestId { get; set; }
        public string subscriptionReference { get; set; }
        public string ckey { get; set; }
    }
}
