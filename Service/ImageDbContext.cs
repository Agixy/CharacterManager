using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Service.Models;

namespace Service
{
    public class ImageDbContext : DbContext
    {
        public DbSet<Image> Images { get; set; }

        public ImageDbContext(DbContextOptions<ImageDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Image>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Image>()
                      .Property(p => p.IsAvatar).HasColumnType("bit");
            modelBuilder.Entity<Image>()
                .Property(p => p.ImageData).HasColumnType("LONGBLOB");
        }
    }
}
