using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class Member
    {
        public Member()
        {
            ActionDetials = new HashSet<ActionDetial>();
            CollectedAuthors = new HashSet<CollectedAuthor>();
            CollectedPublishers = new HashSet<CollectedPublisher>();
            Comments = new HashSet<Comment>();
            CustomerServices = new HashSet<CustomerService>();
        }

        public int MemberId { get; set; }
        public string MemberEmail { get; set; } = null!;
        public string MemberPassword { get; set; } = null!;
        public string MemberName { get; set; } = null!;
        public DateTime? MemberBrithDate { get; set; }
        public string Memberphone { get; set; } = null!;
        public string MemberAddress { get; set; } = null!;
        public int PaymentId { get; set; }
        public int LevelId { get; set; }
        public int CostAmount { get; set; }
        public int Points { get; set; }

        public virtual MemberLevel Level { get; set; } = null!;
        public virtual Payment Payment { get; set; } = null!;
        public virtual ICollection<ActionDetial> ActionDetials { get; set; }
        public virtual ICollection<CollectedAuthor> CollectedAuthors { get; set; }
        public virtual ICollection<CollectedPublisher> CollectedPublishers { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<CustomerService> CustomerServices { get; set; }
    }
}
