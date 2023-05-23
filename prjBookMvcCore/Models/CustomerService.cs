using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class CustomerService
    {
        public int Crmid { get; set; }
        public int MemberId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? ReplyContent { get; set; }
        public string? Ccontent { get; set; }
        public int? Satisfaction { get; set; }
        public int StatusId { get; set; }
        public string? Email { get; set; }
        public string? Note { get; set; }
        public string? Subtitle { get; set; }

        public virtual Member Member { get; set; } = null!;
        public virtual Status Status { get; set; } = null!;
    }
}
