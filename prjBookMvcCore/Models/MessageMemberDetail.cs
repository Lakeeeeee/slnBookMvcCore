using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class MessageMemberDetail
    {
        public int MessageMemberDetailId { get; set; }
        public int? MessageId { get; set; }
        public int? MemberId { get; set; }
        public DateTime? UpdateTime { get; set; }

        public virtual Member? Member { get; set; }
        public virtual Message? Message { get; set; }
    }
}
