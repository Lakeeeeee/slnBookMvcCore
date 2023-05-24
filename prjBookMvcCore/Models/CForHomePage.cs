namespace prjBookMvcCore.Models
{
    public class CForHomePage
    {
        public List<RecommendInformation>? publicationDate { get; set; }

        public List<RecommendInformation>? normal { get; set; }

        public List<RecommendInformation>? commentTimeDate { get; set; }

        public List<RecommendInformation>? commentStar { get; set; }

        public List<CInformation>? cSearchBook { get; set; }
        public List<RecommendInformation>? quantity { get; set; }
    }
}
