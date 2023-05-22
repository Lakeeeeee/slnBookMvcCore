using prjBookMvcCore.Models;
using System.ComponentModel.DataAnnotations;



namespace prjBookMvcCore.ViewModel
{
    public class NewMemberViewModel
    {
        private Member member { get; set; }

        public NewMemberViewModel()
        {
            member = new Member();
        }

        [Required(ErrorMessage ="必填欄位")]
        [EmailAddress]
        public string MemberEmail_P { get { return member.MemberEmail; } set { member.MemberEmail = value; } }
        [Required(ErrorMessage = "密碼為必填欄位")]
        [RegularExpression(@"^(?=.*[A-Z])[A-Za-z0-9]{8,10}$", ErrorMessage = "密碼必須為8到10位的英文字母和數字組合，並至少包含一個大寫字母")]
        public string MemberPassword_P { get { return member.MemberPassword; } set { member.MemberPassword = value; } }
        [Required] public string MemberName_P { get { return member.MemberName; } set { member.MemberName = value; } }
        public DateTime? MemberBrithDate_P { get { return member.MemberBrithDate; } set { member.MemberBrithDate = value; } }
        [Required] public string Memberphone_P { get { return member.Memberphone; } set { member.Memberphone = value; } }
        [Required] public string MemberAddress_P { get { return member.MemberAddress; } set { member.MemberAddress = value; } }
        public bool isSubscribe { get; set; }

   }
}
