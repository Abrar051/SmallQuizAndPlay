using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Models
{
    public class BreaktimequizAnswer
    {
        public long Id { get; set; }
        public string FbId { get; set; }
        public string Answer { get; set; }
        public int Round { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
