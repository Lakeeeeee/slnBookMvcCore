using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class Artical
    {
        public Artical()
        {
            ArticalToBookDetails = new HashSet<ArticalToBookDetail>();
        }

        public int ArticalId { get; set; }
        public DateTime ArticalDate { get; set; }
        public string ArticalTitle { get; set; } = null!;
        public string ArticalDescription { get; set; } = null!;
        public byte[]? ArticalPicture { get; set; }

        public virtual ICollection<ArticalToBookDetail> ArticalToBookDetails { get; set; }
    }
}
