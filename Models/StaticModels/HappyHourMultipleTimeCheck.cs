using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Models.StaticModels
{
    [Keyless]
    public class HappyHourMultipleTimeCheck
    {
        public string result { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string SlNo { get; set; }
        public string remarks { get; set; }
    }
}
