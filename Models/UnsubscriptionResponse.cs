using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Models
{
    public class UnsubscriptionResponse
    {

        public string Id { get; set; }
        public string Href { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public string Remarks { get; set; }

    }
}
