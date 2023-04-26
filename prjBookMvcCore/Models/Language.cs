using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class Language
    {
        public Language()
        {
            Books = new HashSet<Book>();
        }

        public int LanguageId { get; set; }
        public string LanguageName { get; set; } = null!;

        public virtual ICollection<Book> Books { get; set; }
    }
}
