using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.IO;

namespace WebSearcher
{
    public partial class sqliteContext : DbContext
    {
        public sqliteContext()
        {
        }

        public sqliteContext(DbContextOptions<sqliteContext> options)
            : base(options)
        {
        }

        public virtual DbSet<IndexWord> IndexWord { get; set; }
        public virtual DbSet<Posting> Posting { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Filename=sqlite.db");
            }

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<IndexWord>(entity =>
            {
                entity.HasKey(e => e.Word);

                entity.Property(e => e.Word)
                    .HasColumnName("word")
                    .ValueGeneratedNever();
            });

            modelBuilder.Entity<Posting>(entity =>
            {
                entity.HasKey(e => new { e.Word, e.DocumentName });

                entity.Property(e => e.Word).HasColumnName("word");

                entity.Property(e => e.DocumentName).HasColumnName("documentName");

                entity.Property(e => e.Frequency).HasColumnName("frequency");

                entity.Property(e => e.Indexes)
                    .IsRequired()
                    .HasColumnName("indexes");

                entity.HasOne(d => d.WordNavigation)
                    .WithMany(p => p.Posting)
                    .HasForeignKey(d => d.Word)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
        }
    }
}
