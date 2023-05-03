using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class PayStatus
    {
        public int PayStatusId { get; set; }
        public string PayStatusName { get; set; } = null!;
    }
}
