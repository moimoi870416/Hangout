using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Hangout.Models.db
{
    public partial class HangoutContext : DbContext
    {
        public HangoutContext()
        {
        }

        public HangoutContext(DbContextOptions<HangoutContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Favorite> Favorites { get; set; }
        public virtual DbSet<Follow> Follows { get; set; }
        public virtual DbSet<Invite> Invites { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Notice> Notices { get; set; }
        public virtual DbSet<Participant> Participants { get; set; }
        public virtual DbSet<Photo> Photoes { get; set; }
        public virtual DbSet<Relationship> Relationships { get; set; }
        public virtual DbSet<Type> Types { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Chinese_Taiwan_Stroke_CI_AS");

            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("Address");

                entity.Property(e => e.AddressId).HasColumnName("AddressID");

                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.Property(e => e.Road)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comment");

                entity.Property(e => e.CommentId).HasColumnName("CommentID");

                entity.Property(e => e.CommentContent)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.Time)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("Event");

                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.Property(e => e.AddressId).HasColumnName("AddressID");

                entity.Property(e => e.Cover)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Deadline).HasColumnType("smalldatetime");

                entity.Property(e => e.EventContent)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.EventName)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.HostTime).HasColumnType("smalldatetime");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.Property(e => e.TypeId).HasColumnName("TypeID");
            });

            modelBuilder.Entity<Favorite>(entity =>
            {
                entity.HasKey(e => e.FavoritesId)
                    .HasName("PK__Favorite__0E677795F3E985C1");

                entity.Property(e => e.FavoritesId).HasColumnName("FavoritesID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");
            });

            modelBuilder.Entity<Follow>(entity =>
            {
                entity.ToTable("Follow");

                entity.Property(e => e.FollowId).HasColumnName("FollowID");
            });

            modelBuilder.Entity<Invite>(entity =>
            {
                entity.ToTable("Invite");

                entity.Property(e => e.InviteId).HasColumnName("InviteID");
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.ToTable("Member");

                entity.HasIndex(e => e.Account, "UQ__Member__B0C3AC46DEE17D85")
                    .IsUnique();

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.Account)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Birth).HasColumnType("date");

                entity.Property(e => e.Category).HasMaxLength(10);

                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Intro).HasMaxLength(1000);

                entity.Property(e => e.JobTitle).HasMaxLength(10);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("Message");

                entity.Property(e => e.MessageId).HasColumnName("MessageID");

                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.MessageContent)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Status).HasDefaultValueSql("((3))");

                entity.Property(e => e.Time)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Notice>(entity =>
            {
                entity.ToTable("Notice");

                entity.Property(e => e.NoticeId).HasColumnName("NoticeID");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasColumnName("content");
            });

            modelBuilder.Entity<Participant>(entity =>
            {
                entity.ToTable("Participant");

                entity.Property(e => e.ParticipantId).HasColumnName("ParticipantID");

                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.Property(e => e.Motivation).HasMaxLength(200);

                entity.Property(e => e.Status).HasDefaultValueSql("('3')");
            });

            modelBuilder.Entity<Photo>(entity =>
            {
                entity.ToTable("Photo");

                entity.Property(e => e.PhotoId).HasColumnName("PhotoID");

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Relationship>(entity =>
            {
                entity.ToTable("Relationship");

                entity.Property(e => e.RelationshipId).HasColumnName("RelationshipID");
            });

            modelBuilder.Entity<Type>(entity =>
            {
                entity.ToTable("Type");

                entity.Property(e => e.TypeId).HasColumnName("TypeID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
