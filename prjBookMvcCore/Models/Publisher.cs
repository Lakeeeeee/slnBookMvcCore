using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class Publisher
    {
        public Publisher()
        {
            Books = new HashSet<Book>();
            CollectedPublishers = new HashSet<CollectedPublisher>();
            PurchaseDetails = new HashSet<PurchaseDetail>();
        }

        public int PublisherId { get; set; }
        public int? TaxIdnumber { get; set; }
        public string PublisherName { get; set; } = null!;
        public string? PublisherAddress { get; set; }
        public string? PublisherPhone { get; set; }

        public virtual ICollection<Book> Books { get; set; }
        public virtual ICollection<CollectedPublisher> CollectedPublishers { get; set; }
        public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; }
    }
}
