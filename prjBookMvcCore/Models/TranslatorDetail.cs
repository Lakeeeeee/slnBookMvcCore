using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class TranslatorDetail
    {
        public int TranslatorDetailId { get; set; }
        public int TranslatorId { get; set; }
        public int BookId { get; set; }

        public virtual Book Book { get; set; } = null!;
        public virtual Translator Translator { get; set; } = null!;
    }
}
