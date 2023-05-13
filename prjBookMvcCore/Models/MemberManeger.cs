using Microsoft.AspNetCore.Mvc;
using System;
using System.Configuration;
using System.Drawing;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Xml;

namespace prjBookMvcCore.Models
{
    public class MemberManeger
    {

        public void writeWelcomeLetter(Member receiver, BookShopContext content)
        {
            Message welcomeLetter = new Message()
            {
                MessageContent = 
                $"<p>Hi! {receiver.MemberName}!</p>" +
                $"<p>歡迎加入我們的網路書店會員！我們非常高興你決定成為我們的一員，相信你將會在這裡找到許多有趣的閱讀體驗。</p>" +
                $"<p>作為我們的會員，你將享有一系列的優惠與福利，包括專屬的折扣碼、獨家的促銷活動、以及最新書籍的推薦和試讀機會。</p>" +
                $"<p>此外，成為我們的會員還可以享有更多的方便和貼心的服務，例如訂閱通知、簡單的購物流程、多種付款方式、快速的物流配送等等。我們期待與你一同分享我們的閱讀愛好，並為你帶來更多的書香體驗。</p>",
                MessageTypeId = 2,
                MessageTitle = $"歡迎加入讀本會員! {receiver.MemberName}"
            };
            content.Add(welcomeLetter); content.SaveChanges();
            MessageMemberDetail welcomeNewMember = new MessageMemberDetail()
            {
                MessageId = welcomeLetter.MessageId,
                MemberId= receiver.MemberId,
                UpdateTime = DateTime.Now,
                ReadStatu = 0
            };
            content.Add(welcomeNewMember); content.SaveChanges();

        }

    }

}
