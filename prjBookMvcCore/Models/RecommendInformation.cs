namespace prjBookMvcCore.Models
{
    public class RecommendInformation
    {
        public Book ?book { get; set; }

        public BookDiscount ?bookDiscount { get; set; }
        public BookDiscountDetail ?bookDiscountDetail { get; set; }
        public Comment ?comment { get; set; }
    }
}
