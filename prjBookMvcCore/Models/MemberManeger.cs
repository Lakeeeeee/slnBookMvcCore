using Microsoft.AspNetCore.Mvc;
using System;
using System.Configuration;
using System.Drawing;
using System.Net;
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
                $"<p>歡迎加入我們的網路書店會員！你現在是我們的  <span>{content.MemberLevels.Where(x=>x.LevelId== receiver.LevelId).FirstOrDefault().LevelName}</span>  會員了，相信你將會在這裡找到許多有趣的閱讀體驗。</p>" +
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
        public void write註冊會員禮Letter(Member receiver, BookShopContext content)
        {
            Message Letter = new Message()
            {  
                MessageTypeId = 2,
                MessageTitle = $"註冊會員禮已發送",
                MessageContent =
                $"<p>Hi! {receiver.MemberName}!</p>" +
                $"<p>歡迎加入我們的網路書店會員！我們非常高興你決定成為我們的一員，相信你將會在這裡找到許多有趣的閱讀體驗。</p>" +
                $"<p>註冊會員禮已發送，請至<a href=\"myCoupons\">我的Coupons</a>確認，並請注意使用期限。</p>" +
                $"<a href=\"../../Promotions/Promotions促銷活動\">快來使用吧!!</a>",
            };
            content.Add(Letter); content.SaveChanges();
            MessageMemberDetail MemberMessage = new MessageMemberDetail()
            {
                MessageId = Letter.MessageId,
                MemberId = receiver.MemberId,
                UpdateTime = DateTime.Now,
                ReadStatu = 0
            };
            content.Add(MemberMessage); content.SaveChanges();
        }
        
        //TODO：有訂電子報的會員，定時發送訊息
        public void write本月優惠Letter(Member receiver, BookShopContext content)
        {
            Message Letter = new Message()
            {
                MessageTypeId = 2,
                MessageTitle = $"本月優惠",
                MessageContent =
                $"<p>Hi! {receiver.MemberName}!</p>" +
                $"<p>本月優惠活動開跑，<a href=\"../../Promotions/Promotions領取月優惠\">快來領取使用吧!!</a>",
            };
            content.Add(Letter); content.SaveChanges();
            MessageMemberDetail MemberMessage = new MessageMemberDetail()
            {
                MessageId = Letter.MessageId,
                MemberId = receiver.MemberId,
                UpdateTime = DateTime.Now,
                ReadStatu = 0
            };
            content.Add(MemberMessage); content.SaveChanges();
        }

        public void writeComplanintReply(CustomerService complaint, IConfiguration config, BookShopContext db)
        {
            string mailContent = $"親愛的讀者：<br>" +
                $"您於 {complaint.UpdateDate} 時，所提出的問題如下： <p>{complaint.Ccontent}</p>" +
                $"目前的處理狀態為： <p>{db.Statuses.Find(complaint.StatusId).StatusName}</p>" +
                $"<p>{db.Statuses.Find(complaint.StatusId).StatusDescription}</p>" +
                $"請您耐心等待後續客服人員的聯絡，謝謝。" +
                $"<p>讀本購書平台</p>";
            string mailSubject = "[讀本] 您的問題已被接受";
            string SmtpServer = "smtp.gmail.com";
            string GoogleMailUserID = config["GoogleMailUserID"];
            string GoogleMailUserPwd = config["GoogleMailUserPwd"];
            int port = 587;
            MailMessage mms = new MailMessage();
            mms.From = new MailAddress(GoogleMailUserID);
            mms.Subject = mailSubject;
            mms.Body = mailContent;
            mms.IsBodyHtml = true;
            mms.SubjectEncoding = Encoding.UTF8;
            mms.To.Add(new MailAddress(complaint.Email));
            using (SmtpClient client = new SmtpClient(SmtpServer, port))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(GoogleMailUserID, GoogleMailUserPwd);
                client.Send(mms);
            }
        }
    }
}
