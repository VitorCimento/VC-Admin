using Microsoft.EntityFrameworkCore;
using VC_Admin.Domain.Entities;

namespace VC_Admin.Infrastructure.Contexts
{
    public class AppDbContext : DbContext
    {
        // Declarar os DBSets aqui!
        public DbSet<User> Users => Set<User>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.HasIndex(u => u.Email)
                    .IsUnique();
                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(200);
                entity.Property(u => u.Username)
                    .HasMaxLength(100);
                entity.Property(u => u.PasswordHash)
                    .IsRequired();
                entity.Property(u => u.CreatedAt)
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("now()");
                entity.Property(u => u.UpdatedAt)
                    .HasColumnType("timestamp with time zone");
            });
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetUpdatedAt();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            SetUpdatedAt();
            return base.SaveChanges();
        }

        private void SetUpdatedAt()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();
            var now = DateTime.UtcNow;

            foreach (var entry in entries.Where(x => x.State == EntityState.Modified))
                entry.Property(nameof(BaseEntity.UpdatedAt)).CurrentValue = now;
        }
    }
}
