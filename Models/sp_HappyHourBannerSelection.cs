using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Models
{
    [Keyless]
    public class sp_HappyHourBannerSelection
    {
        public int? BannerNo { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public int? SlNo { get; set; }
    }
}
