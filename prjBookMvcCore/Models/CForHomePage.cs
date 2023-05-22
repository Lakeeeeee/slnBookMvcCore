namespace prjBookMvcCore.Models
{
    public class CForHomePage
    {
        public List<RecommendInformation>? publicationDate { get; set; }

        public List<RecommendInformation>? normal { get; set; }

        public List<RecommendInformation>? commentTimeDate { get; set; }

        public List<RecommendInformation>? searchResult { get; set; }

        public List<Book>? c書 { get; set; }
        public List<Author>? c作者 { get; set; }
        public List<Translator>? c譯者 { get; set; }
        public List<Painter>? c繪者 { get; set; }
        public List<Publisher>? c出版社 { get; set; }
        public List<Category>? c分類 { get; set; }
    }
}
