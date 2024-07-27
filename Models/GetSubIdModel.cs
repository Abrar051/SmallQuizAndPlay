using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Models
{
    public class SubIdExtraParams
    {
        public string cancellationReason { get; set; }
    }
    public class GetSubIdModel
    {
        public int id { get; set; }
        public string createdAt { get; set; }
        public string modifiedAt { get; set; }
        public string subscriptionRequestId { get; set; }
        public string requesterId { get; set; }
        public string serviceId { get; set; }
        public string paymentType { get; set; }
        public string subscriptionType { get; set; }
        public string amountQueryUrl { get; set; }
        public string amount { get; set; }
        public string firstPaymentAmount { get; set; }
        public string maxCapRequired { get; set; }
        public string maxCapAmount { get; set; }
        public string frequency { get; set; }
        public string startDate { get; set; }
        public string expiryDate { get; set; }
        public string merchantId { get; set; }
        public string payerType { get; set; }
        public string payer { get; set; }
        public string currency { get; set; }
        public string nextPaymentDate { get; set; }
        public string status { get; set; }
        public string subscriptionReference { get; set; }
        public SubIdExtraParams extraParams { get; set; }
        public string cancelledBy { get; set; }
        public string cancelledTime { get; set; }
        public bool enabled { get; set; }
        public bool expired { get; set; }
        public string rrule { get; set; }
        public bool active { get; set; }
    }
}
