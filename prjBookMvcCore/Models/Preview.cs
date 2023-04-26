using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class Preview
    {
        public int PreviewId { get; set; }
        public int BookId { get; set; }
        public string? PreviewImagePath1 { get; set; }
        public string? PreviewImagePath2 { get; set; }
        public string? PreviewImagePath3 { get; set; }
        public string? PreviewImagePath4 { get; set; }

        public virtual Book Book { get; set; } = null!;
    }
}
