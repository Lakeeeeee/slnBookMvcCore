using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace prjBookMvcCore.Models
{
    public partial class BookShopContext : DbContext
    {
        public BookShopContext()
        {
        }

        public BookShopContext(DbContextOptions<BookShopContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Action> Actions { get; set; } = null!;
        public virtual DbSet<ActionDetial> ActionDetials { get; set; } = null!;
        public virtual DbSet<Artical> Articals { get; set; } = null!;
        public virtual DbSet<ArticalToBookDetail> ArticalToBookDetails { get; set; } = null!;
        public virtual DbSet<Author> Authors { get; set; } = null!;
        public virtual DbSet<AuthorDetail> AuthorDetails { get; set; } = null!;
        public virtual DbSet<Book> Books { get; set; } = null!;
        public virtual DbSet<BookDiscount> BookDiscounts { get; set; } = null!;
        public virtual DbSet<BookDiscountDetail> BookDiscountDetails { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<CategoryDetail> CategoryDetails { get; set; } = null!;
        public virtual DbSet<CollectedAuthor> CollectedAuthors { get; set; } = null!;
        public virtual DbSet<CollectedPublisher> CollectedPublishers { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<CustomerService> CustomerServices { get; set; } = null!;
        public virtual DbSet<Discount> Discounts { get; set; } = null!;
        public virtual DbSet<DiscountDetail> DiscountDetails { get; set; } = null!;
        public virtual DbSet<Language> Languages { get; set; } = null!;
        public virtual DbSet<Member> Members { get; set; } = null!;
        public virtual DbSet<MemberLevel> MemberLevels { get; set; } = null!;
        public virtual DbSet<Message> Messages { get; set; } = null!;
        public virtual DbSet<MessageMemberDetail> MessageMemberDetails { get; set; } = null!;
        public virtual DbSet<MessageSubscribe> MessageSubscribes { get; set; } = null!;
        public virtual DbSet<MessageType> MessageTypes { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<OrderDiscount> OrderDiscounts { get; set; } = null!;
        public virtual DbSet<OrderDiscountDetail> OrderDiscountDetails { get; set; } = null!;
        public virtual DbSet<Painter> Painters { get; set; } = null!;
        public virtual DbSet<PainterDetail> PainterDetails { get; set; } = null!;
        public virtual DbSet<PayStatus> PayStatuses { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Preview> Previews { get; set; } = null!;
        public virtual DbSet<Publisher> Publishers { get; set; } = null!;
        public virtual DbSet<Purchase> Purchases { get; set; } = null!;
        public virtual DbSet<PurchaseDetail> PurchaseDetails { get; set; } = null!;
        public virtual DbSet<Shipment> Shipments { get; set; } = null!;
        public virtual DbSet<ShippingStatus> ShippingStatuses { get; set; } = null!;
        public virtual DbSet<Status> Statuses { get; set; } = null!;
        public virtual DbSet<SubCategory> SubCategories { get; set; } = null!;
        public virtual DbSet<Translator> Translators { get; set; } = null!;
        public virtual DbSet<TranslatorDetail> TranslatorDetails { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=BookShop;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Action>(entity =>
            {
                entity.ToTable("Action");

                entity.Property(e => e.ActionId).HasColumnName("ActionID");

                entity.Property(e => e.ActionName).HasMaxLength(50);
            });

            modelBuilder.Entity<ActionDetial>(entity =>
            {
                entity.HasKey(e => e.ActionToBookId);

                entity.ToTable("ActionDetial");

                entity.Property(e => e.ActionToBookId).HasColumnName("ActionToBookID");

                entity.Property(e => e.ActionId).HasColumnName("ActionID");

                entity.Property(e => e.BookId).HasColumnName("BookID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.HasOne(d => d.Action)
                    .WithMany(p => p.ActionDetials)
                    .HasForeignKey(d => d.ActionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ActionDetial_Action");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.ActionDetials)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ActionDetial_Books");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.ActionDetials)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ActionDetial_Member");
            });

            modelBuilder.Entity<Artical>(entity =>
            {
                entity.ToTable("Artical");

                entity.Property(e => e.ArticalId).HasColumnName("ArticalID");

                entity.Property(e => e.ArticalDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<ArticalToBookDetail>(entity =>
            {
                entity.HasKey(e => e.ArticalDetailId);

                entity.ToTable("ArticalToBookDetail");

                entity.Property(e => e.ArticalDetailId).HasColumnName("ArticalDetailID");

                entity.Property(e => e.ArticalId).HasColumnName("ArticalID");

                entity.Property(e => e.BookId).HasColumnName("BookID");

                entity.HasOne(d => d.Artical)
                    .WithMany(p => p.ArticalToBookDetails)
                    .HasForeignKey(d => d.ArticalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ArticalToBookDetail_Artical");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.ArticalToBookDetails)
                    .HasForeignKey(d => d.BookId)
                    .HasConstraintName("FK_ArticalToBookDetail_Books");
            });

            modelBuilder.Entity<Author>(entity =>
            {
                entity.ToTable("Author");

                entity.Property(e => e.AuthorId).HasColumnName("AuthorID");

                entity.Property(e => e.AuthorName).HasMaxLength(50);
            });

            modelBuilder.Entity<AuthorDetail>(entity =>
            {
                entity.ToTable("AuthorDetail");

                entity.Property(e => e.AuthorDetailId).HasColumnName("AuthorDetailID");

                entity.Property(e => e.AuthorId).HasColumnName("AuthorID");

                entity.Property(e => e.BookId).HasColumnName("BookID");

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.AuthorDetails)
                    .HasForeignKey(d => d.AuthorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AuthorDetail_Author");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.AuthorDetails)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AuthorDetail_Books");
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.Property(e => e.BookId).HasColumnName("BookID");

                entity.Property(e => e.BindingMethod).HasMaxLength(50);

                entity.Property(e => e.Dimensions).HasMaxLength(50);

                entity.Property(e => e.Isbn)
                    .HasMaxLength(13)
                    .HasColumnName("ISBN");

                entity.Property(e => e.LanguageId).HasColumnName("LanguageID");

                entity.Property(e => e.Pages).HasMaxLength(50);

                entity.Property(e => e.PublicationDate).HasColumnType("date");

                entity.Property(e => e.PublisherId).HasColumnName("PublisherID");

                entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.LanguageId)
                    .HasConstraintName("FK_Books_Language");

                entity.HasOne(d => d.Publisher)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.PublisherId)
                    .HasConstraintName("FK_Books_Publisher");
            });

            modelBuilder.Entity<BookDiscount>(entity =>
            {
                entity.ToTable("BookDiscount");

                entity.Property(e => e.BookDiscountId).HasColumnName("BookDiscountID");

                entity.Property(e => e.BookDiscountAmount).HasColumnType("numeric(3, 2)");

                entity.Property(e => e.BookDiscountName).HasMaxLength(10);
            });

            modelBuilder.Entity<BookDiscountDetail>(entity =>
            {
                entity.ToTable("BookDiscountDetail");

                entity.Property(e => e.BookDiscountDetailId).HasColumnName("BookDiscountDetailID");

                entity.Property(e => e.BookDiscountEndDate).HasColumnType("date");

                entity.Property(e => e.BookDiscountId).HasColumnName("BookDiscountID");

                entity.Property(e => e.BookDiscountStartDate).HasColumnType("date");

                entity.Property(e => e.BookId).HasColumnName("BookID");

                entity.HasOne(d => d.BookDiscount)
                    .WithMany(p => p.BookDiscountDetails)
                    .HasForeignKey(d => d.BookDiscountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BookDiscountDetail_BookDiscount");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.BookDiscountDetails)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BookDiscountDetail_Books");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CategoryName).HasMaxLength(10);
            });

            modelBuilder.Entity<CategoryDetail>(entity =>
            {
                entity.ToTable("CategoryDetail");

                entity.Property(e => e.CategoryDetailId).HasColumnName("CategoryDetailID");

                entity.Property(e => e.BookId).HasColumnName("BookID");

                entity.Property(e => e.SubCategoryId).HasColumnName("SubCategoryID");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.CategoryDetails)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CategoryDetail_Books");

                entity.HasOne(d => d.SubCategory)
                    .WithMany(p => p.CategoryDetails)
                    .HasForeignKey(d => d.SubCategoryId)
                    .HasConstraintName("FK_CategoryDetail_SubCategory");
            });

            modelBuilder.Entity<CollectedAuthor>(entity =>
            {
                entity.ToTable("CollectedAuthor");

                entity.Property(e => e.CollectedAuthorId).HasColumnName("CollectedAuthorID");

                entity.Property(e => e.AuthorId).HasColumnName("AuthorID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.CollectedAuthors)
                    .HasForeignKey(d => d.AuthorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CollectedAuthor_Author");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.CollectedAuthors)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CollectedAuthor_Member");
            });

            modelBuilder.Entity<CollectedPublisher>(entity =>
            {
                entity.ToTable("CollectedPublisher");

                entity.Property(e => e.CollectedPublisherId).HasColumnName("CollectedPublisherID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.PublisherId).HasColumnName("PublisherID");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.CollectedPublishers)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CollectedPublisher_Member");

                entity.HasOne(d => d.Publisher)
                    .WithMany(p => p.CollectedPublishers)
                    .HasForeignKey(d => d.PublisherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CollectedPublisher_Publisher");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comment");

                entity.Property(e => e.CommentId).HasColumnName("CommentID");

                entity.Property(e => e.BookId).HasColumnName("BookID");

                entity.Property(e => e.CommentTime).HasColumnType("datetime");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comment_Books");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.MemberId)
                    .HasConstraintName("FK_Comment_Member");
            });

            modelBuilder.Entity<CustomerService>(entity =>
            {
                entity.HasKey(e => e.Crmid);

                entity.ToTable("CustomerService");

                entity.Property(e => e.Crmid).HasColumnName("CRMID");

                entity.Property(e => e.Ccontent).HasColumnName("CContent");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.UpdateDate).HasColumnType("date");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.CustomerServices)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerService_Member");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.CustomerServices)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomerService_Status");
            });

            modelBuilder.Entity<Discount>(entity =>
            {
                entity.ToTable("Discount");

                entity.Property(e => e.DiscountId)
                    .ValueGeneratedNever()
                    .HasColumnName("DiscountID");

                entity.Property(e => e.DiscountAmount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.DiscountName).HasMaxLength(50);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.IsActive).HasComment("1");

                entity.Property(e => e.StartDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<DiscountDetail>(entity =>
            {
                entity.ToTable("Discount_Detail");

                entity.Property(e => e.DiscountDetailId).HasColumnName("DiscountDetailID");

                entity.Property(e => e.BookId).HasColumnName("BookID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.DiscountId).HasColumnName("DiscountID");
            });

            modelBuilder.Entity<Language>(entity =>
            {
                entity.ToTable("Language");

                entity.Property(e => e.LanguageId).HasColumnName("LanguageID");

                entity.Property(e => e.LanguageName).HasMaxLength(10);
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.ToTable("Member");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.LevelId)
                    .HasColumnName("LevelID")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.MemberAddress).HasMaxLength(50);

                entity.Property(e => e.MemberBrithDate).HasColumnType("date");

                entity.Property(e => e.MemberEmail).HasMaxLength(50);

                entity.Property(e => e.MemberName).HasMaxLength(10);

                entity.Property(e => e.MemberPassword).HasMaxLength(10);

                entity.Property(e => e.Memberphone).HasMaxLength(10);

                entity.Property(e => e.PaymentId)
                    .HasColumnName("PaymentID")
                    .HasDefaultValueSql("((6))");

                entity.HasOne(d => d.Level)
                    .WithMany(p => p.Members)
                    .HasForeignKey(d => d.LevelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Member_MemberLevel");

                entity.HasOne(d => d.Payment)
                    .WithMany(p => p.Members)
                    .HasForeignKey(d => d.PaymentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Member_Payment");
            });

            modelBuilder.Entity<MemberLevel>(entity =>
            {
                entity.HasKey(e => e.LevelId);

                entity.ToTable("MemberLevel");

                entity.Property(e => e.LevelId).HasColumnName("LevelID");

                entity.Property(e => e.LevelDescription).HasMaxLength(50);

                entity.Property(e => e.LevelDiscount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.LevelName).HasMaxLength(10);
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("Message");

                entity.Property(e => e.MessageId).HasColumnName("MessageID");

                entity.Property(e => e.MessageTitle).HasMaxLength(50);

                entity.Property(e => e.MessageTypeId).HasColumnName("MessageTypeID");

                entity.HasOne(d => d.MessageType)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.MessageTypeId)
                    .HasConstraintName("FK_Message_MessageType");
            });

            modelBuilder.Entity<MessageMemberDetail>(entity =>
            {
                entity.ToTable("MessageMemberDetail");

                entity.Property(e => e.MessageMemberDetailId).HasColumnName("MessageMemberDetailID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.MessageId).HasColumnName("MessageID");

                entity.Property(e => e.UpdateTime).HasColumnType("date");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.MessageMemberDetails)
                    .HasForeignKey(d => d.MemberId)
                    .HasConstraintName("FK_MessageMemberDetail_Member");

                entity.HasOne(d => d.Message)
                    .WithMany(p => p.MessageMemberDetails)
                    .HasForeignKey(d => d.MessageId)
                    .HasConstraintName("FK_MessageMemberDetail_Message");
            });

            modelBuilder.Entity<MessageSubscribe>(entity =>
            {
                entity.ToTable("MessageSubscribe");

                entity.Property(e => e.MessageSubscribeId).HasColumnName("MessageSubscribeID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.MessageTypeId).HasColumnName("MessageTypeID");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.MessageSubscribes)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MessageSubscribe_Member");

                entity.HasOne(d => d.MessageType)
                    .WithMany(p => p.MessageSubscribes)
                    .HasForeignKey(d => d.MessageTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MessageSubscribe_MessageType");
            });

            modelBuilder.Entity<MessageType>(entity =>
            {
                entity.ToTable("MessageType");

                entity.Property(e => e.MessageTypeId).HasColumnName("MessageTypeID");

                entity.Property(e => e.MessageTypeName).HasMaxLength(50);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.OrderId)
                    .ValueGeneratedNever()
                    .HasColumnName("OrderID");

                entity.Property(e => e.DiscountAmount).HasColumnType("money");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.OrderDate).HasColumnType("date");

                entity.Property(e => e.PayStatusId).HasColumnName("PayStatusID");

                entity.Property(e => e.PaymentId).HasColumnName("PaymentID");

                entity.Property(e => e.ShipAddr).HasMaxLength(50);

                entity.Property(e => e.ShipmentId).HasColumnName("ShipmentID");

                entity.Property(e => e.ShippingStatusId).HasColumnName("ShippingStatusID");

                entity.Property(e => e.TotalPay).HasColumnType("money");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Member1");

                entity.HasOne(d => d.PayStatus)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.PayStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_PayStatus");

                entity.HasOne(d => d.Payment)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.PaymentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Payment");

                entity.HasOne(d => d.Shipment)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ShipmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Shipment");

                entity.HasOne(d => d.ShippingStatus)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ShippingStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_ShippingStatus");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("OrderDetail");

                entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");

                entity.Property(e => e.BookId).HasColumnName("BookID");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetail_Books");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetail_Order");
            });

            modelBuilder.Entity<OrderDiscount>(entity =>
            {
                entity.ToTable("OrderDiscount");

                entity.Property(e => e.OrderDiscountId).HasColumnName("OrderDiscountID");

                entity.Property(e => e.OrderDiscountAmount).HasColumnType("numeric(3, 0)");

                entity.Property(e => e.OrderDiscountCode).HasMaxLength(15);

                entity.Property(e => e.OrderDiscountDescprtion).HasMaxLength(50);

                entity.Property(e => e.OrderDiscountnName).HasMaxLength(50);
            });

            modelBuilder.Entity<OrderDiscountDetail>(entity =>
            {
                entity.ToTable("OrderDiscountDetail");

                entity.Property(e => e.OrderDiscountDetailId).HasColumnName("OrderDiscountDetailID");

                entity.Property(e => e.IsOrderDiscountUse)
                    .HasMaxLength(1)
                    .HasDefaultValueSql("(N'N')");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.OrderDiscountEndDate).HasColumnType("date");

                entity.Property(e => e.OrderDiscountId).HasColumnName("OrderDiscountID");

                entity.Property(e => e.OrderDiscountStartDate).HasColumnType("date");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.OrderDiscountDetails)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDiscountDetail_Member");

                entity.HasOne(d => d.OrderDiscount)
                    .WithMany(p => p.OrderDiscountDetails)
                    .HasForeignKey(d => d.OrderDiscountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDiscountDetail_OrderDiscount");
            });

            modelBuilder.Entity<Painter>(entity =>
            {
                entity.ToTable("Painter");

                entity.Property(e => e.PainterId).HasColumnName("PainterID");

                entity.Property(e => e.PainterName).HasMaxLength(50);
            });

            modelBuilder.Entity<PainterDetail>(entity =>
            {
                entity.ToTable("PainterDetail");

                entity.Property(e => e.PainterDetailId).HasColumnName("PainterDetailID");

                entity.Property(e => e.BookId).HasColumnName("BookID");

                entity.Property(e => e.PainterId).HasColumnName("PainterID");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.PainterDetails)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PainterDetail_Books");

                entity.HasOne(d => d.Painter)
                    .WithMany(p => p.PainterDetails)
                    .HasForeignKey(d => d.PainterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PainterDetail_Painter");
            });

            modelBuilder.Entity<PayStatus>(entity =>
            {
                entity.ToTable("PayStatus");

                entity.Property(e => e.PayStatusId).HasColumnName("PayStatusID");

                entity.Property(e => e.PayStatusName).HasMaxLength(50);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.PaymentId).HasColumnName("PaymentID");

                entity.Property(e => e.PaymentName).HasMaxLength(50);
            });

            modelBuilder.Entity<Preview>(entity =>
            {
                entity.ToTable("Preview");

                entity.Property(e => e.PreviewId).HasColumnName("PreviewID");

                entity.Property(e => e.BookId).HasColumnName("BookID");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.Previews)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Preview_Books");
            });

            modelBuilder.Entity<Publisher>(entity =>
            {
                entity.ToTable("Publisher");

                entity.Property(e => e.PublisherId).HasColumnName("PublisherID");

                entity.Property(e => e.PublisherAddress).HasMaxLength(50);

                entity.Property(e => e.PublisherName).HasMaxLength(50);

                entity.Property(e => e.PublisherPhone).HasMaxLength(50);

                entity.Property(e => e.TaxIdnumber).HasColumnName("taxIDnumber");
            });

            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.ToTable("Purchase");

                entity.Property(e => e.PurchaseId).HasColumnName("PurchaseID");

                entity.Property(e => e.ArrivedData).HasColumnType("date");

                entity.Property(e => e.PurchaseDate).HasColumnType("date");
            });

            modelBuilder.Entity<PurchaseDetail>(entity =>
            {
                entity.ToTable("PurchaseDetail");

                entity.Property(e => e.PurchaseDetailId).HasColumnName("PurchaseDetailID");

                entity.Property(e => e.BookId).HasColumnName("BookID");

                entity.Property(e => e.Isbn)
                    .HasMaxLength(13)
                    .HasColumnName("ISBN");

                entity.Property(e => e.PublisherId).HasColumnName("PublisherID");

                entity.Property(e => e.PurchaseId).HasColumnName("PurchaseID");

                entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.PurchaseDetails)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PurchaseDetail_Books");

                entity.HasOne(d => d.Publisher)
                    .WithMany(p => p.PurchaseDetails)
                    .HasForeignKey(d => d.PublisherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PurchaseDetail_Store");

                entity.HasOne(d => d.Purchase)
                    .WithMany(p => p.PurchaseDetails)
                    .HasForeignKey(d => d.PurchaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PurchaseDetail_Purchase");
            });

            modelBuilder.Entity<Shipment>(entity =>
            {
                entity.ToTable("Shipment");

                entity.Property(e => e.ShipmentId).HasColumnName("ShipmentID");

                entity.Property(e => e.Freight)
                    .HasColumnType("money")
                    .HasColumnName("freight");

                entity.Property(e => e.ShipmentName)
                    .HasMaxLength(15)
                    .IsFixedLength();
            });

            modelBuilder.Entity<ShippingStatus>(entity =>
            {
                entity.ToTable("ShippingStatus");

                entity.Property(e => e.ShippingStatusId).HasColumnName("ShippingStatusID");

                entity.Property(e => e.ShippingStatusName).HasMaxLength(50);
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("Status");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.StatusName).HasMaxLength(50);
            });

            modelBuilder.Entity<SubCategory>(entity =>
            {
                entity.ToTable("SubCategory");

                entity.Property(e => e.SubCategoryId).HasColumnName("SubCategoryID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.SubCategoryName).HasMaxLength(10);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.SubCategories)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SubCategory_Category");
            });

            modelBuilder.Entity<Translator>(entity =>
            {
                entity.ToTable("Translator");

                entity.Property(e => e.TranslatorId).HasColumnName("TranslatorID");

                entity.Property(e => e.TranslatorName).HasMaxLength(50);
            });

            modelBuilder.Entity<TranslatorDetail>(entity =>
            {
                entity.ToTable("TranslatorDetail");

                entity.Property(e => e.TranslatorDetailId).HasColumnName("TranslatorDetailID");

                entity.Property(e => e.BookId).HasColumnName("BookID");

                entity.Property(e => e.TranslatorId).HasColumnName("TranslatorID");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.TranslatorDetails)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TranslatorDetail_Books");

                entity.HasOne(d => d.Translator)
                    .WithMany(p => p.TranslatorDetails)
                    .HasForeignKey(d => d.TranslatorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TranslatorDetail_Translator");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
