using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class AuthorDetail
    {
        public int AuthorDetailId { get; set; }
        public int AuthorId { get; set; }
        public int BookId { get; set; }

        public virtual Author Author { get; set; } = null!;
        public virtual Book Book { get; set; } = null!;
    }
}
