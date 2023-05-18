using Microsoft.Extensions.Logging;
using prjBookMvcCore.Models;
using System.ComponentModel.DataAnnotations;
using System.Net;


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
        [Required]
        public string MemberPassword_P { get { return member.MemberPassword; } set { member.MemberPassword= value; } }
        [Required] public string MemberName_P { get { return member.MemberName; } set { member.MemberName = value; } }
        [Required]
        public DateTime? MemberBrithDate_P { get { return member.MemberBrithDate; } set { member.MemberBrithDate = value; } }
        [Required] public string Memberphone_P { get { return member.Memberphone; } set { member.Memberphone = value; } }
        [Required] public string MemberAddress_P { get { return member.MemberAddress; } set { member.MemberAddress = value; } }
        public bool isSubscribe { get; set; }

   }
}
