using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class Comment
    {
        public int CommentId { get; set; }
        public int? MemberId { get; set; }
        public int BookId { get; set; }
        public string? CommentText { get; set; }
        public int Stars { get; set; }
        public DateTime CommentTime { get; set; }

        public virtual Book Book { get; set; } = null!;
        public virtual Member? Member { get; set; }
    }
}
