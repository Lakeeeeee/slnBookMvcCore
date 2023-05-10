namespace prjBookMvcCore.Models
{
    public class CInformation
    {
        public Book ?book { get; set; }
        public Language ?language { get; set; }
        public Publisher ?publisher { get; set; }
        public Category ?category { get; set; }
        public SubCategory ?subCategory { get; set; }
        public Translator ?translator { get; set; }
        public Painter ?painter { get; set; }
        public Author ?author { get; set; }

        public List<CommentInformation> ?commentInformations { get; set; }

    }
}
