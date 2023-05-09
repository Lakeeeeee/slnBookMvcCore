using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class MessageSubscribe
    {
        public int MessageSubscribeId { get; set; }
        public int? MessageTypeId { get; set; }
        public int? MemberId { get; set; }

        public virtual Member? Member { get; set; }
        public virtual MessageType? MessageType { get; set; }
    }
}
