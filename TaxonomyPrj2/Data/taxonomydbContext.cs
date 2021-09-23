using System;
using DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TaxonomyPrj2.Models;

#nullable disable

namespace TaxonomyPrj2.Data
{
    public partial class taxonomydbContext : IdentityDbContext<User>
    {
        public taxonomydbContext()
        {
        }

        public taxonomydbContext(DbContextOptions<taxonomydbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Organism> Organisms { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=taxonomydb;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");
            base.OnModelCreating(modelBuilder); //Добавил для миграции
            modelBuilder.Entity<Organism>(entity =>
            {
                entity.HasIndex(e => e.CategoryId, "IX_Organisms_CategoryId");

                entity.Property(e => e.StartResearch).HasColumnType("date");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Organisms)
                    .HasForeignKey(d => d.CategoryId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
