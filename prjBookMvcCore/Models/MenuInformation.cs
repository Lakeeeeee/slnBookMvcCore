namespace prjBookMvcCore.Models
{
    public class MenuInformation
    {
        public List<Artical>? articals { get; set; }
        public List<MenuItem>? menuItems { get; set; }
        public List<Category>? categories { get; set; }

        public List<SubCategory>? subCategories { get; set; }
    }
}
