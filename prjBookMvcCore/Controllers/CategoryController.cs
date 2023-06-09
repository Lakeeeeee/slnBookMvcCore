﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjBookMvcCore.Models;
using X.PagedList;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace prjBookMvcCore.Controllers
{
    public class CategoryController : Controller
    {
        BookShopContext db = new();

        public IActionResult 分類頁面(int id, int subid, int page = 1, string sort = "PublicationDate")
        {
            int categoryID = id;
            int subcategoryID = subid;
            int itemsPerPage = 20;//每頁只顯示20個

            var query = from b in db.Books
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
                            作者ID = b.AuthorDetails.Select(x => x.Author.AuthorId).FirstOrDefault(),
                            分類 = b.CategoryDetails.Select(x => x.SubCategory.Category.CategoryName).FirstOrDefault(),
                            分類ID = b.CategoryDetails.Select(x => x.SubCategory.CategoryId).FirstOrDefault(),
                            子分類 = b.CategoryDetails.Select(x => x.SubCategory.SubCategoryName).FirstOrDefault(),
                            子分類ID = b.CategoryDetails.Select(x => x.SubCategory.SubCategoryId).FirstOrDefault(),
                            折扣 = b.BookDiscountDetails.Where(x => x.BookDiscountStartDate < DateTime.Now & x.BookDiscountEndDate > DateTime.Now).Select(x => x.BookDiscount.BookDiscountAmount).FirstOrDefault(),
                            出版日期 = b.PublicationDate,
                        };
            //書籍取得分類ID
            if (categoryID != 0)
            {
                query = query.Where(sc => sc.分類ID == categoryID);
            }
           if (subcategoryID != 0)
            {
                query = query.Where(sc => sc.子分類ID == subcategoryID);
            }
            //篩選條件
            switch (sort)
            {
                case "PublicationDate":
                    query = query.OrderByDescending(b => b.出版日期);
                    break;
                case "Price":
                    query = query.OrderBy(b => b.定價 * b.折扣);
                    break;
            }
            //頁面顯示控制
            int totalItems = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);
            int offset = (page - 1) * itemsPerPage;
            var books = query.Skip(offset).Take(itemsPerPage);
            var pagedBooks = query.Skip(offset).Take(itemsPerPage).ToList();

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
                Author a = new Author { AuthorName = item.作者 , AuthorId = item.作者ID};
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
                CurrentPage = page,
                TotalPages = totalPages,
                categoryId = categoryID, 
                subcategoryId= subcategoryID,
                SortBy = sort
            };
            return View(menuInformation);
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
