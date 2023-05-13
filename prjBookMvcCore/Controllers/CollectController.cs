using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using prjBookMvcCore.Models;

namespace prjBookMvcCore.Controllers
{
    public class CollectController : Controller
    {
        BookShopContext db = new();
        public string collectAuthor(int memberID, int authorID)
        {
            bool isSuccess = true;
            var query = from ca in db.CollectedAuthors
                        where ca.MemberId == memberID && ca.AuthorId == authorID
                        select ca;
            if (query.Count() != 0)
            {
                isSuccess = false;
            }
            else
            {
                CollectedAuthor newCa = new CollectedAuthor
                {
                    AuthorId = authorID,
                    MemberId = memberID,
                };
                db.CollectedAuthors.Add(newCa);
                db.SaveChanges();
            }
            string jsonData = JsonConvert.SerializeObject(isSuccess);
            return jsonData;
        }
        public string collectPublisher(int memberID, int publisherID)
        {
            bool isSuccess = true;
            var query = from cp in db.CollectedPublishers
                        where cp.MemberId == memberID && cp.PublisherId == publisherID
                        select cp;
            if (query.Count() != 0)
            {
                isSuccess = false;
            }
            else
            {
                CollectedPublisher newCp = new CollectedPublisher
                {
                    MemberId = memberID,
                    PublisherId = publisherID,
                };
                db.CollectedPublishers.Add(newCp);
                db.SaveChanges();
            }
            string jsonData = JsonConvert.SerializeObject(isSuccess);
            return jsonData;
        }
    }
}
