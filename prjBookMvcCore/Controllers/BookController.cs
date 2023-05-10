﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prjBookMvcCore.Models;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace prjBookMvcCore.Controllers
{
    public class BookController : Controller
    {
        BookShopContext db = new();

        public IActionResult BookInformation()
        {
            int bookId = 123;
            var query = from b in db.Books
                        where b.BookId == bookId
                        select new
                        {
                            書本ID = b.BookId,
                            書名 = b.BookTitle,
                            ISBN = b.Isbn,
                            作者 = b.AuthorDetails.Select(x => x.Author.AuthorName).FirstOrDefault(),
                            譯者 = b.TranslatorDetails.Select(x => x.Translator.TranslatorName).FirstOrDefault(),
                            繪者 = b.PainterDetails.Select(x => x.Painter.PainterName).FirstOrDefault(),
                            出版社 = b.Publisher.PublisherName,
                            出版日期 = b.PublicationDate,
                            語言 = b.Language.LanguageName,
                            定價 = b.UnitPrice,
                            封裝方法 = b.BindingMethod,
                            頁數 = b.Pages,
                            規格 = b.Dimensions,
                            庫存 = b.UnitInStock,
                            分類 = b.CategoryDetails.Select(x => x.SubCategory.Category.CategoryName).FirstOrDefault(),
                            子分類 = b.CategoryDetails.Select(x => x.SubCategory.SubCategoryName).FirstOrDefault(),
                            路徑 = b.CoverPath,
                            詳細資料 = b.Introduction,
                            作者介紹 = b.AboutAuthor,
                            內容簡介 = b.ContentIntro,
                            章節試閱 = b.TryContent,
                            各界推薦 = b.Endorsements,
                            作者序 = b.Foreward,
                            目錄 = b.TableContainer,
                        };
            foreach (var item in query)
            {
                Book b = new Book
                {
                    BookId = item.書本ID,
                    BookTitle = item.書名,
                    PublicationDate = item.出版日期,
                    UnitPrice = item.定價,
                    Isbn = item.ISBN,
                    BindingMethod = item.封裝方法,
                    Pages = item.頁數,
                    Dimensions = item.規格,
                    UnitInStock = item.庫存,
                    CoverPath = item.路徑,
                    Introduction = item.詳細資料,
                    AboutAuthor = item.作者介紹,
                    ContentIntro = item.內容簡介,
                    TryContent = item.章節試閱,
                    Endorsements = item.各界推薦,
                    Foreward = item.作者序,
                    TableContainer = item.目錄,
                };
                Publisher p = new Publisher { PublisherName = item.出版社 };
                Language l = new Language { LanguageName = item.語言 };
                Category c = new Category { CategoryName = item.分類 };
                SubCategory sc = new SubCategory { SubCategoryName = item.子分類 };
                Translator t = new Translator { TranslatorName = item.譯者 };
                Painter pt = new Painter { PainterName = item.繪者 };
                Author a = new Author { AuthorName = item.作者 };

                CInformation newBook = new CInformation
                {
                    book = b,
                    publisher = p,
                    language = l,
                    category = c,
                    subCategory = sc,
                    author = a,
                    translator = t,
                    painter = pt,
                };
                return View(newBook);
            }
            return View();
        }
    }
}
