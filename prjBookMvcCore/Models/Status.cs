using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class Status
    {
        public Status()
        {
            CustomerServices = new HashSet<CustomerService>();
        }

        public int StatusId { get; set; }
        public string StatusName { get; set; } = null!;
        public string StatusDescription { get; set; } = null!;

        public virtual ICollection<CustomerService> CustomerServices { get; set; }
    }
}
