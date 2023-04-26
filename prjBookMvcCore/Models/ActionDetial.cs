using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class ActionDetial
    {
        public int ActionToBookId { get; set; }
        public int BookId { get; set; }
        public int ActionId { get; set; }
        public int MemberId { get; set; }

        public virtual Action Action { get; set; } = null!;
        public virtual Book Book { get; set; } = null!;
        public virtual Member Member { get; set; } = null!;
    }
}
