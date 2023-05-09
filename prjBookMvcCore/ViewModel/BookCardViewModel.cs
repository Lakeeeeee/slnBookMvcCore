using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using prjBookMvcCore.Models;
using System;

namespace prjBookMvcCore.Models
{
    public class BooksCardModel
    {
            public DbSet<Book> ?Books { get; set; }
            public DbSet<Author>? Authors { get; set; }
            public DbSet<AuthorDetail>? AuthorDetails { get; set; }
            public DbSet<BookDiscount>? BookDiscounts { get; set; }
            public DbSet<BookDiscountDetail>? BookDiscountDetails { get; set; }

        public class Author
        {
            public int AuthorId { get; set; }
            public string AuthorName { get; set; } = string.Empty;
        }

        public class AuthorDetail
        {
            public int AuthorId { get; set; }
            public int BookId { get; set; }
        }

        public class BookDiscount
        {
            public int DiscountId { get; set; }
            public string DiscountName { get; set; } = string.Empty;
            public decimal DiscountAmount { get; set; }
        }
        public class BookDiscountDetail
        {
            public int DiscountId { get; set; }
            public int BookId { get; set; }
    }
        public class Book
        {
            public int BookId { get; set; }
            public string BookTitle { get; set; } = string.Empty;
            public decimal UnitPrice { get; set; }
            public int LanguageId { get; set; }
            public DateTime PublicationDate { get; set; }
            public List<Author> ?Authors { get; set; } 
            public List<BookDiscount> ?BookDiscounts { get; set; }
        }

    }
}
