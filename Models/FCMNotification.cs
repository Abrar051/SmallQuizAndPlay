using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizMaster.Models
{
    public class FCMNotification
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string ImageUrl { get; set; }
    }
}
