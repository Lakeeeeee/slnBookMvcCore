using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class PainterDetail
    {
        public int PainterDetailId { get; set; }
        public int PainterId { get; set; }
        public int BookId { get; set; }

        public virtual Book Book { get; set; } = null!;
        public virtual Painter Painter { get; set; } = null!;
    }
}
