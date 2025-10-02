using AlltOmHundar.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace AlltOmHundar.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets - representerar tabeller i databasen
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<PrivateMessage> PrivateMessages { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupMember> GroupMembers { get; set; }
        public DbSet<GroupMessage> GroupMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User konfiguration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Username).IsUnique();
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Bio).HasMaxLength(500);
            });

            // Category konfiguration
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
            });

            // Topic konfiguration
            modelBuilder.Entity<Topic>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.HasOne(e => e.Category)
                    .WithMany(c => c.Topics)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Post konfiguration
            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Content).IsRequired();

                entity.HasOne(e => e.Topic)
                    .WithMany(t => t.Posts)
                    .HasForeignKey(e => e.TopicId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.User)
                    .WithMany(u => u.Posts)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Self-referencing för trädstruktur (svar på inlägg)
                entity.HasOne(e => e.ParentPost)
                    .WithMany(p => p.Replies)
                    .HasForeignKey(e => e.ParentPostId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Reaction konfiguration
            modelBuilder.Entity<Reaction>(entity =>
            {
                entity.HasKey(e => e.Id);

                // En användare kan bara ha en reaktion per inlägg
                entity.HasIndex(e => new { e.PostId, e.UserId }).IsUnique();

                entity.HasOne(e => e.Post)
                    .WithMany(p => p.Reactions)
                    .HasForeignKey(e => e.PostId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.User)
                    .WithMany(u => u.Reactions)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Report konfiguration
            modelBuilder.Entity<Report>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Reason).IsRequired().HasMaxLength(500);

                entity.HasOne(e => e.Post)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(e => e.PostId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.ReportedByUser)
                    .WithMany(u => u.Reports)
                    .HasForeignKey(e => e.ReportedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // PrivateMessage konfiguration
            modelBuilder.Entity<PrivateMessage>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Content).IsRequired();

                entity.HasOne(e => e.Sender)
                    .WithMany(u => u.SentMessages)
                    .HasForeignKey(e => e.SenderId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Receiver)
                    .WithMany(u => u.ReceivedMessages)
                    .HasForeignKey(e => e.ReceiverId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Group konfiguration
            modelBuilder.Entity<Group>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);

                entity.HasOne(e => e.CreatedByUser)
                    .WithMany(u => u.CreatedGroups)
                    .HasForeignKey(e => e.CreatedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // GroupMember konfiguration
            modelBuilder.Entity<GroupMember>(entity =>
            {
                entity.HasKey(e => e.Id);

                // En användare kan bara vara medlem en gång per grupp
                entity.HasIndex(e => new { e.GroupId, e.UserId }).IsUnique();

                entity.HasOne(e => e.Group)
                    .WithMany(g => g.Members)
                    .HasForeignKey(e => e.GroupId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.User)
                    .WithMany(u => u.GroupMemberships)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // GroupMessage konfiguration
            modelBuilder.Entity<GroupMessage>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Content).IsRequired();

                entity.HasOne(e => e.Group)
                    .WithMany(g => g.Messages)
                    .HasForeignKey(e => e.GroupId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Sender)
                    .WithMany()
                    .HasForeignKey(e => e.SenderId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Seed data - grundläggande kategorier
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Hundsnack", Description = "Allmänt hunprat", DisplayOrder = 1 },
                new Category { Id = 2, Name = "Hundgodissnack", Description = "Allt om hundgodis och belöningar", DisplayOrder = 2 },
                new Category { Id = 3, Name = "Träningssnack", Description = "Träning och uppfostran", DisplayOrder = 3 },
                new Category { Id = 4, Name = "Valpsnack", Description = "Allt om valpar", DisplayOrder = 4 }
            );
        }
    }
}