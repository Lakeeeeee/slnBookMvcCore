using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class CollectedAuthor
    {
        public int CollectedAuthorId { get; set; }
        public int MemberId { get; set; }
        public int AuthorId { get; set; }
        public DateTime? Catime { get; set; }

        public virtual Author Author { get; set; } = null!;
        public virtual Member Member { get; set; } = null!;
    }
}
