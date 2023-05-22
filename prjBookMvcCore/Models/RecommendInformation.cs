namespace prjBookMvcCore.Models
{
    public class RecommendInformation
    {
        public Book ?book { get; set; }
        public Author ?author { get; set; }
        public Painter ?painter { get; set; }
        public Translator ?translator { get; set; }
        public Publisher ?publisher { get; set; }
        public BookDiscount ?bookDiscount { get; set; }
        public BookDiscountDetail ?bookDiscountDetail { get; set; }
        public Comment ?comment { get; set; }
        public OrderDetail? orderDetail { get; set; }
    }
}
