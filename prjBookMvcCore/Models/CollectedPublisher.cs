using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class CollectedPublisher
    {
        public int CollectedPublisherId { get; set; }
        public int MemberId { get; set; }
        public int PublisherId { get; set; }
        public DateTime? Cptime { get; set; }

        public virtual Member Member { get; set; } = null!;
        public virtual Publisher Publisher { get; set; } = null!;
    }
}
