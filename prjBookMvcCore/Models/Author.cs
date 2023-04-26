using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class Author
    {
        public Author()
        {
            AuthorDetails = new HashSet<AuthorDetail>();
            CollectedAuthors = new HashSet<CollectedAuthor>();
        }

        public int AuthorId { get; set; }
        public string? AuthorName { get; set; }

        public virtual ICollection<AuthorDetail> AuthorDetails { get; set; }
        public virtual ICollection<CollectedAuthor> CollectedAuthors { get; set; }
    }
}
