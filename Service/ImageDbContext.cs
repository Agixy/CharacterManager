using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Service.Models;

namespace Service
{
    public class ImageDbContext : DbContext
    {
        public DbSet<Image> Images { get; set; }
        private readonly IConfiguration Configuration;

        public ImageDbContext()
        {
            Configuration = new ConfigurationBuilder().AddJsonFile("secrets.json").Build();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(Configuration["ConnectionStrings:ImageDbContext"]);
        }

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
