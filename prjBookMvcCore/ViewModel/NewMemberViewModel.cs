using prjBookMvcCore.Models;

namespace prjBookMvcCore.ViewModel
{
    public class NewMemberViewModel
    {
        public int MemberId_p { get; set; }
        public string MemberEmail_p { get; set; } = null!;
        public string MemberPassword_p { get; set; } = null!;
        public string MemberName_p { get; set; } = null!;
        public DateTime? MemberBrithDate_p { get; set; }
        public string Memberphone_p { get; set; } = null!;
        public string MemberAddress_p { get; set; } = null!;
        public int PaymentId_p { get; set; }
        public int LevelId_p { get; set; }
        public int CostAmount_p { get; set; }
        public int Points_p { get; set; }
    }
}
