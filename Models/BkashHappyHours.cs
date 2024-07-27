using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.Models
{
    public class BkashHappyHours
    {
       
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; } 
        public string ServiceName { get; set; }

    }
}
