﻿namespace prjBookMvcCore.Models
{
    public class BooksCardViewModel
    {
        public int ArticalID { get; set; }
        public string? ArticalTitle { get; set; }
        public string? ArticalDescription { get; set; }
        public byte[]? ArticalPicture { get; set; }
        public int BookId { get; set; }
        public string? BookTitle { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime PublicationDate { get; set; }
        public string? CoverPath { get; set; }
        public int? AuthorId { get; set; }
        public string? AuthorName { get; set; }
        public string? BookDiscountName { get; set; }
        public decimal BookDiscountAmount { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int SubCategoryId { get; set; }
        public string ?SubCategoryName { get; set; }

        private Book _book { get; set; }
        //private AuthorDetail _authorDetail;
        //private Author _author;
        //private BookDiscountDetail _bookDiscountDetail;
        //private BookDiscount _bookDiscount;
        //private Language _language;
        //public Book book
        //{
        //    get { return _book; }
        //    set { _book = value; }
        //}
        //public AuthorDetail authorDetail
        //{
        //    get { return _authorDetail; }
        //    set { _authorDetail = value; }
        //}
        //public Author author
        //{
        //    get { return _author; }
        //    set { _author = value; }
        //}
        //public BookDiscountDetail bookDiscountDetail
        //{
        //    get { return _bookDiscountDetail; }
        //    set { _bookDiscountDetail = value; }
        //}
        //public BookDiscount bookDiscount
        //{
        //    get { return _bookDiscount; }
        //    set { _bookDiscount = value; }
        //}
        //public Language language
        //{
        //    get { return _language; }
        //    set { _language = value; }
        //}
        //public BooksCardViewModel()
        //{
        //    _book = new Book();
        //    _authorDetail = new AuthorDetail();
        //    _author = new Author();
        //    _bookDiscountDetail = new BookDiscountDetail();
        //    _bookDiscount = new BookDiscount();
        //    _language = new Language();
        //}
        //public int BookId 
        //{ get { return _book.BookId; }
        //   set { _book.BookId  = value; }
        //}
        //public string BookTitle
        //{
        //    get {return _book.BookTitle; }
        //    set {_book.BookTitle=value; }
        //}
        //public DateTime PublicationDate 
        //{
        //    get { return _book.PublicationDate; }
        //    set { _book.PublicationDate = value; }
        //}
        //public int LanguageId 
        //{ get { return _language.LanguageId ; }
        //   set { _language.LanguageId = value; }
        //}
        //public decimal UnitPrice 
        //{
        //    get { return _book.UnitPrice; }
        //    set { _book.UnitPrice = value; }
        //}
        //public string CoverPath
        //{
        //    get { return _book.CoverPath; }
        //    set { _book.CoverPath = value; }
        //}
        //public int AuthorId 
        //{
        //    get { return _author.AuthorId; }
        //    set { _authorDetail.AuthorId = value; }
        //}
        //public string? AuthorName 
        //{
        //    get { return _author.AuthorName; }
        //    set { _author.AuthorName = value; }
        //}
        //public string BookDiscountName 
        //{
        //    get { return _bookDiscount.BookDiscountName; }
        //    set { _bookDiscount.BookDiscountName = value; }
        //}
        //public decimal BookDiscountAmount
        //{
        //    get { return _bookDiscount.BookDiscountAmount; }
        //    set { _bookDiscount.BookDiscountAmount = value; }
        //}
    }
}