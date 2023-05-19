namespace prjBookMvcCore.Models
{
    public class checkoutInformation
    {
        public Member? member { get; set; }

        public MemberLevel? memberlevel { get; set; }
        public OrderDiscount? orderdiscount { get; set; }

    }
}
