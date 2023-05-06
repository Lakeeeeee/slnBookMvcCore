using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class Message
    {
        public Message()
        {
            MessageMemberDetails = new HashSet<MessageMemberDetail>();
        }

        public int MessageId { get; set; }
        public int? MessageTypeId { get; set; }
        public string? MessageTitle { get; set; }
        public string? MessageContent { get; set; }

        public virtual MessageType? MessageType { get; set; }
        public virtual ICollection<MessageMemberDetail> MessageMemberDetails { get; set; }
    }
}
