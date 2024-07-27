using System;
using System.Collections.Generic;

#nullable disable

namespace QuizMaster.Models.WapPortal
{
    public partial class TblQpFriendList
    {
        public long Id { get; set; }
        public string UserFbId { get; set; }
        public string FriendFbId { get; set; }
        public int? IsActive { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}
