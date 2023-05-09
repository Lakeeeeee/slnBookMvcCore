using Newtonsoft.Json.Linq;
using prjBookMvcCore.Models;

namespace prjBookMvcCore.Models
{
    public class BooksCardModel
    {
        public int BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int LanguageId { get; set; }
        public DateTime PublicationDate { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public int DiscountId { get; set; }
        public string DiscountName { get; set; } = string.Empty;
        public decimal DiscountAmount { get; set; }
    }
}
