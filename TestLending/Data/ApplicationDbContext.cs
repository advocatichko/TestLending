using Microsoft.EntityFrameworkCore;
using TestLending.Models;

namespace TestLending.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<CrmLead> CrmLeads { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Set default schema to CRM
            modelBuilder.HasDefaultSchema("CRM");
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            // Конфигурация модели CRM_Lead
            modelBuilder.Entity<CrmLead>(entity =>
            {
                entity.ToTable("CRM_Lead"); // Removed explicit schema to use default schema
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Message).HasMaxLength(500);
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.Property(e => e.Source).HasMaxLength(50);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.IsProcessed).HasDefaultValue(false);
            });
        }
    }
}
