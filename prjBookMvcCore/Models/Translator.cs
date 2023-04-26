using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class Translator
    {
        public Translator()
        {
            TranslatorDetails = new HashSet<TranslatorDetail>();
        }

        public int TranslatorId { get; set; }
        public string? TranslatorName { get; set; }

        public virtual ICollection<TranslatorDetail> TranslatorDetails { get; set; }
    }
}
