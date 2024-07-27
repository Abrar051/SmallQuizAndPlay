using System;
using System.Collections.Generic;

namespace QuizMaster.Models
{
    public partial class TblMsisdnQuizBkash
    {
        public int Id { get; set; }
        public string SourceUrl { get; set; }
        public string ServiceRequest { get; set; }
        public string Msisdn { get; set; }
        public string UaprofileUrl { get; set; }
        public string HsManufacturer { get; set; }
        public string HsModel { get; set; }
        public string HsDim { get; set; }
        public string Os { get; set; }
        public string DeviceIp { get; set; }
        public string Apn { get; set; }
        public string PortalFullnShort { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
