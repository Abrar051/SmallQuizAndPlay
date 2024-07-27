using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Models
{
    public class UserInfo
    {
        public long? Id { get; set; }
        public string MSISDN { get; set; }
        public string Name { get; set; }
        public string SourceUrl { get; set; }
        public string CKey { get; set; }
        public int? IsSignup { get; set; }
        public Nullable<System.DateTime> TimeStamp { get; set; }
        public string FPToken { get; set; }
    }
}
