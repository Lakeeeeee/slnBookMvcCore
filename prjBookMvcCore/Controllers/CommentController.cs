using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using prjBookMvcCore.Models;

namespace prjBookMvcCore.Controllers
{
    public class CommentController : Controller
    {
        BookShopContext db = new();
        public string writeComment(int bookID, int memberID, string text, int stars)
        {
            bool isSuccess = true;
            var query = from c in db.Comments
                        where c.BookId == bookID && c.MemberId == memberID
                        select c;
            if (query.Count() != 0)
            {
                isSuccess = false;
            }
            else
            {
                Comment comment = new Comment()
                {
                    BookId = bookID,
                    MemberId = memberID,
                    CommentText = text,
                    Stars = stars,
                    CommentTime = DateTime.Now,
                };
                db.Comments.Add(comment);
                db.SaveChanges();
            }
            string jsonData = JsonConvert.SerializeObject(isSuccess);
            return jsonData;
        }
    }
}
