using System;
using System.Collections.Generic;

#nullable disable

namespace QuizMaster.Models
{
    public partial class CheckoutUserData
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Msisdn { get; set; }
        public string ServiceName { get; set; }
        public string PaymentId { get; set; }
        public string TokenType { get; set; }
        public string Token { get; set; }
        public string CreateTime { get; set; }
        public string UpdateTime { get; set; }
        public string OrgLogo { get; set; }
        public string OrgName { get; set; }
        public string TransactionStatus { get; set; }
        public string Amount { get; set; }
        public string Currency { get; set; }
        public string Intent { get; set; }
        public string MerchantInvoiceNumber { get; set; }
        public string TrxId { get; set; }
        public string Completed { get; set; }
        public string UserAgent { get; set; }
        public string Ip { get; set; }
        public string HsManufac { get; set; }
        public string HsMod { get; set; }
        public string HsDim { get; set; }
        public string HsOs { get; set; }
        public string SourceUrl { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
