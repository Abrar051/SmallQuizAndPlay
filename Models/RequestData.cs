using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Models
{
    public class RequestData
    {
        public string amount { get; set; }
        public string intent { get; set; }
        public string currency { get; set; }
    }
}
