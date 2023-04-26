using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class Painter
    {
        public Painter()
        {
            PainterDetails = new HashSet<PainterDetail>();
        }

        public int PainterId { get; set; }
        public string? PainterName { get; set; }

        public virtual ICollection<PainterDetail> PainterDetails { get; set; }
    }
}
