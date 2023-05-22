namespace prjBookMvcCore.Models
{
   public class MenuInformation
    {
        public List<Artical>? articals { get; set; }
        public List<MenuItem>? menuItems { get; set; }
        public List<Category>? categories { get; set; }
        public List<SubCategory>? subCategories { get; set; }
        public List<OrderDetail>? orderDetails { get; set; }
        public int CurrentPage { get; set; } // Add this property
        public int TotalPages { get; set; }
        public int categoryId { get; set; }
        public int bookDiscountId { get; set; }
        public int subcategoryId { get; set; }
    }
}