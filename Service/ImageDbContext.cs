using Microsoft.EntityFrameworkCore;
using Service.Models;

namespace Service
{
    public class ImageDbContext : DbContext
    {
        public DbSet<Image> Images { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Image>().Property(e => e.Id).ValueGeneratedOnAdd();

            // lub:
        //    [Column("Active", TypeName = "bit")]
        //[DefaultValue(false)]

        modelBuilder.Entity<Image>()
                  .Property(p => p.IsAvatar).HasColumnType("bit");
            modelBuilder.Entity<Image>()
                .Property(p => p.ImageData).HasColumnType("LONGBLOB");
        }
    }
}
