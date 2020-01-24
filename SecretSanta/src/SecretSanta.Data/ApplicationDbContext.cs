using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace SecretSanta.Data
{
    public class ApplicationDbContext : DbContext
    {

#nullable disable //CS8616 Non-nullable property is uninitialized
        public DbSet<User> Users { get; set; }
        public DbSet<Gift> Gifts { get; set; }
        public DbSet<Group> Groups { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }
#nullable enable 

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            IHttpContextAccessor httpContextAccessor) : base(options)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder?.Entity<GroupUser>().HasKey(gu => new {gu.GroupId, gu.UserId});

            modelBuilder?.Entity<GroupUser>()
                .HasOne(gu => gu.User)
                .WithMany(u => u.GroupUsers)
                .HasForeignKey(u => u.UserId);

            modelBuilder?.Entity<GroupUser>()
                .HasOne(gu => gu.Group)
                .WithMany(u => u.GroupUsers)
                .HasForeignKey(g => g.GroupId);
        }

        public override int SaveChanges()
        {
            AddFingerPrinting();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddFingerPrinting();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void AddFingerPrinting()
        {
            var modified = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified);
            var added = ChangeTracker.Entries().Where(e => e.State == EntityState.Added);

            foreach (var entry in added)
            {
                if (entry.Entity is FingerPrintEntityBase fingerPrintEntry)
                {
                    fingerPrintEntry.CreatedOn = DateTime.UtcNow;
                    fingerPrintEntry.CreatedBy = HttpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier).Value ?? "";
                    fingerPrintEntry.ModifiedOn = DateTime.UtcNow;
                    fingerPrintEntry.ModifiedBy = HttpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier).Value ?? "";
                }
            }

            foreach (var entry in modified)
            {
                if (entry.Entity is FingerPrintEntityBase fingerPrintEntry)
                { 
                    fingerPrintEntry.ModifiedOn = DateTime.UtcNow;
                    fingerPrintEntry.ModifiedBy = HttpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier).Value ?? "";
                }
            }

        }
    }
}
