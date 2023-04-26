using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class ArticalToBookDetail
    {
        public int ArticalDetailId { get; set; }
        public int ArticalId { get; set; }
        public int? BookId { get; set; }

        public virtual Artical Artical { get; set; } = null!;
        public virtual Book? Book { get; set; }
    }
}
