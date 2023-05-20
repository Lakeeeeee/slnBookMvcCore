namespace prjBookMvcCore.Models
{
    public class MenuItem
    {
        public Book? book { get; set; }
        public Author? author { get; set; }
        public BookDiscount? bookDiscount { get; set; }
        public BookDiscountDetail? bookDiscountDetail { get; set; }
        public Category? category { get; set; }
        public SubCategory? subCategory { get; set; }
    }
}
