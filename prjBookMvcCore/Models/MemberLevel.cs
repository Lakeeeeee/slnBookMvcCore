using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class MemberLevel
    {
        public MemberLevel()
        {
            Members = new HashSet<Member>();
        }

        public int LevelId { get; set; }
        public string LevelName { get; set; } = null!;
        public int MinCost { get; set; }
        public decimal DiscountAmount { get; set; }
        public string LevelDescription { get; set; } = null!;

        public virtual ICollection<Member> Members { get; set; }
    }
}
