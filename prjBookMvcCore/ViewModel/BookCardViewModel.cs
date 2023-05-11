namespace prjBookMvcCore.Models
{
    public class BooksCardViewModel
    {
        public int BookId { get; set; }
        public string? BookTitle { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime PublicationDate { get; set; }
        public string? CoverPath { get; set; }
        public int? AuthorId { get; set; }
        public string? AuthorName { get; set; }
        public string? BookDiscountName { get; set; }
        public decimal BookDiscountAmount { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int SubCategoryId { get; set; }
        public string ?SubCategoryName { get; set; }
    }
}