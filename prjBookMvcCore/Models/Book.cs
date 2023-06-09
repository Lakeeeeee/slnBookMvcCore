﻿using System;
using System.Collections.Generic;

namespace prjBookMvcCore.Models
{
    public partial class Book
    {
        public Book()
        {
            ActionDetials = new HashSet<ActionDetial>();
            ArticalToBookDetails = new HashSet<ArticalToBookDetail>();
            AuthorDetails = new HashSet<AuthorDetail>();
            BookDiscountDetails = new HashSet<BookDiscountDetail>();
            CategoryDetails = new HashSet<CategoryDetail>();
            Comments = new HashSet<Comment>();
            OrderDetails = new HashSet<OrderDetail>();
            PainterDetails = new HashSet<PainterDetail>();
            Previews = new HashSet<Preview>();
            PurchaseDetails = new HashSet<PurchaseDetail>();
            TranslatorDetails = new HashSet<TranslatorDetail>();
        }

        public int BookId { get; set; }
        public string BookTitle { get; set; } = null!;
        public string? AboutAuthor { get; set; }
        public int? PublisherId { get; set; }
        public DateTime PublicationDate { get; set; }
        public int? LanguageId { get; set; }
        public decimal UnitPrice { get; set; }
        public string? Introduction { get; set; }
        public string? ContentIntro { get; set; }
        public string? TryContent { get; set; }
        public string? Endorsements { get; set; }
        public string? Foreward { get; set; }
        public string? TableContainer { get; set; }
        public string Isbn { get; set; } = null!;
        public string BindingMethod { get; set; } = null!;
        public string Pages { get; set; } = null!;
        public string Dimensions { get; set; } = null!;
        public int? UnitInStock { get; set; }
        public string? CoverPath { get; set; }

        public virtual Language? Language { get; set; }
        public virtual Publisher? Publisher { get; set; }
        public virtual ICollection<ActionDetial> ActionDetials { get; set; }
        public virtual ICollection<ArticalToBookDetail> ArticalToBookDetails { get; set; }
        public virtual ICollection<AuthorDetail> AuthorDetails { get; set; }
        public virtual ICollection<BookDiscountDetail> BookDiscountDetails { get; set; }
        public virtual ICollection<CategoryDetail> CategoryDetails { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<PainterDetail> PainterDetails { get; set; }
        public virtual ICollection<Preview> Previews { get; set; }
        public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; }
        public virtual ICollection<TranslatorDetail> TranslatorDetails { get; set; }
    }
}
