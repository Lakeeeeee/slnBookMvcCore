using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjBookMvcCore.Models;

namespace prjBookMvcCore.Controllers
{
    public class CategoryController : Controller
    {
        BookShopContext db = new();
        //TODO:(書玉)分頁controller發法改寫
        public IActionResult 分類頁面(int categoryID)
        {
            if(categoryID == 0)
            {
                var books = from b in db.Books
                            join sd in db.CategoryDetails
                            on b.BookId equals sd.BookId
                            join sc in db.SubCategories
                            on sd.SubCategoryId equals sc.SubCategoryId
                            select new
                            {
                                書本ID = b.BookId,
                                書名 = b.BookTitle,
                                定價 = b.UnitPrice,
                                路徑 = b.CoverPath,
                                作者 = b.AuthorDetails.Select(x => x.Author.AuthorName).FirstOrDefault(),
                                分類 = b.CategoryDetails.Select(x => x.SubCategory.Category.CategoryName).FirstOrDefault(),
                                分類ID = b.CategoryDetails.Select(x => x.SubCategory.CategoryId).FirstOrDefault(),
                                子分類 = b.CategoryDetails.Select(x => x.SubCategory.SubCategoryName).FirstOrDefault(),
                                子分類ID = b.CategoryDetails.Select(x => x.SubCategory.SubCategoryId).FirstOrDefault(),
                                折扣 = b.BookDiscountDetails.Select(x => x.BookDiscount.BookDiscountAmount).FirstOrDefault(),
                                出版日期 = b.PublicationDate,
                            };
                List<MenuItem> menuItems = new List<MenuItem>();
                foreach (var item in books)
                {
                    Book b = new Book
                    {
                        BookTitle = item.書名,
                        BookId = item.書本ID,
                        UnitPrice = item.定價,
                        CoverPath = item.路徑,
                        PublicationDate = item.出版日期,
                    };
                    BookDiscount bd = new BookDiscount { BookDiscountAmount = item.折扣 };
                    Author a = new Author { AuthorName = item.作者 };
                    Category c = new Category { CategoryName = item.分類, CategoryId = item.分類ID };
                    SubCategory sc = new SubCategory { SubCategoryName = item.子分類, SubCategoryId = item.子分類ID };
                    MenuItem tmp = new MenuItem();
                    tmp.book = b;
                    tmp.bookDiscount = bd;
                    tmp.category = c;
                    tmp.subCategory = sc;
                    tmp.author = a;
                    menuItems.Add(tmp);
                }

                List<Category> categories = getCategories();
                List<SubCategory> subCategories = getSubCategories();

                MenuInformation menuInformation = new MenuInformation
                {
                    categories = categories,
                    subCategories = subCategories,
                    menuItems = menuItems,
                };
                return View(menuInformation);
            }
            else
            {
                var books = from b in db.Books
                            join sd in db.CategoryDetails
                            on b.BookId equals sd.BookId
                            join sc in db.SubCategories
                            on sd.SubCategoryId equals sc.SubCategoryId
                            where sc.CategoryId == categoryID
                            select new
                            {
                                書本ID = b.BookId,
                                書名 = b.BookTitle,
                                定價 = b.UnitPrice,
                                路徑 = b.CoverPath,
                                作者 = b.AuthorDetails.Select(x => x.Author.AuthorName).FirstOrDefault(),
                                分類 = b.CategoryDetails.Select(x => x.SubCategory.Category.CategoryName).FirstOrDefault(),
                                分類ID = b.CategoryDetails.Select(x => x.SubCategory.CategoryId).FirstOrDefault(),
                                子分類 = b.CategoryDetails.Select(x => x.SubCategory.SubCategoryName).FirstOrDefault(),
                                子分類ID = b.CategoryDetails.Select(x => x.SubCategory.SubCategoryId).FirstOrDefault(),
                                折扣 = b.BookDiscountDetails.Select(x => x.BookDiscount.BookDiscountAmount).FirstOrDefault(),
                                出版日期 = b.PublicationDate,
                            };
                List<MenuItem> menuItems = new List<MenuItem>();
                foreach (var item in books)
                {
                    Book b = new Book
                    {
                        BookTitle = item.書名,
                        BookId = item.書本ID,
                        UnitPrice = item.定價,
                        CoverPath = item.路徑,
                        PublicationDate = item.出版日期,
                    };
                    BookDiscount bd = new BookDiscount { BookDiscountAmount = item.折扣 };
                    Author a = new Author { AuthorName = item.作者 };
                    Category c = new Category { CategoryName = item.分類, CategoryId = item.分類ID };
                    SubCategory sc = new SubCategory { SubCategoryName = item.子分類, SubCategoryId = item.子分類ID };
                    MenuItem tmp = new MenuItem();
                    tmp.book = b;
                    tmp.bookDiscount = bd;
                    tmp.category = c;
                    tmp.subCategory = sc;
                    tmp.author = a;
                    menuItems.Add(tmp);
                }

                List<Category> categories = getCategories();
                List<SubCategory> subCategories = getSubCategories();

                MenuInformation menuInformation = new MenuInformation
                {
                    categories = categories,
                    subCategories = subCategories,
                    menuItems = menuItems,
                };
                return View(menuInformation);
            }
           
        }

        public List<Category> getCategories()
        {
            using (var db = new BookShopContext())
            {
                var query = from c in db.Categories
                            select c;
                List<Category> categories = new List<Category>();
                foreach (var c in query)
                {
                    categories.Add(c);
                }
                return categories;
            }
        }

        public List<SubCategory> getSubCategories()
        {
            using (var db = new BookShopContext())
            {
                var query = from sc in db.SubCategories
                            select sc;
                List<SubCategory> subCategories = new List<SubCategory>();
                foreach (var sc in query)
                {
                    subCategories.Add(new SubCategory
                    {
                        SubCategoryId = sc.SubCategoryId,
                        SubCategoryName = sc.SubCategoryName,
                        CategoryId = sc.CategoryId
                    });
                }
                return subCategories;
            }
        }

    }
}
