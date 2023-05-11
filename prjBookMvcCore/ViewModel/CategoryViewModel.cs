using prjBookMvcCore.Models;

namespace prjBookMvcCore.ViewModel
{
    internal class CategoryViewModel
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int SubCategoryId { get; set; }
        public string? SubCategoryName { get; set; }
    }
}