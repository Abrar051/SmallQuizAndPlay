using System;
using System.Collections.Generic;

#nullable disable

namespace QuizMaster.Models.BkashPaymentGateWay
{
    public partial class SessionDatum
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Msisdn { get; set; }
        public string ServiceName { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
