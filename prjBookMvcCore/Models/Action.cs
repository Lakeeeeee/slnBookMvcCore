using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class Action
    {
        public Action()
        {
            ActionDetials = new HashSet<ActionDetial>();
        }

        public int ActionId { get; set; }
        public string ActionName { get; set; } = null!;
        public string? ActionDescription { get; set; }

        public virtual ICollection<ActionDetial> ActionDetials { get; set; }
    }
}
