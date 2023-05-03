using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class MessageType
    {
        public MessageType()
        {
            MessageSubscribes = new HashSet<MessageSubscribe>();
            Messages = new HashSet<Message>();
        }

        public int MessageTypeId { get; set; }
        public string? MessageTypeName { get; set; }
        public string? MessageTypeDescription { get; set; }

        public virtual ICollection<MessageSubscribe> MessageSubscribes { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}
