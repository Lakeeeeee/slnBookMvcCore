using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace prjBookMvcCore.Models
{
    public partial class Artical
    {
        public Artical()
        {
            ArticalToBookDetails = new HashSet<ArticalToBookDetail>();
        }

        public int ArticalId { get; set; }

        [DisplayName("更新日期")]
        public DateTime ArticalDate { get; set; }

        [DisplayName("文章標題")]
        public string ArticalTitle { get; set; } = null!;
        public string ArticalDescription { get; set; } = null!;
        public byte[]? ArticalPicture { get; set; }

        public virtual ICollection<ArticalToBookDetail> ArticalToBookDetails { get; set; }
    }
}
