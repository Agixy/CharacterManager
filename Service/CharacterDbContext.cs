using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Service.Models;

namespace Service
{
    public class CharacterDbContext : DbContext
    {
        public DbSet<Character> Characters { get; set; }
        public DbSet<Origin> Origins { get; set; }
        public DbSet<Breed> Breeds { get; set; }
        public DbSet<Personality> Pertonalities { get; set; }
        public DbSet<Appearance> Appearances { get; set; }
        public DbSet<TypeOfCharacter> TypeOfCharacters { get; set; }
        public DbSet<Orientation> Orientations { get; set; }
        public DbSet<AlignmentChart> AlignmentCharts { get; set; }
        public DbSet<Image> Images { get; set; }

        public CharacterDbContext(DbContextOptions<CharacterDbContext> options) 
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Character>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Appearance>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Personality>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Origin>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<AlignmentChart>().Property(e => e.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<Character>().HasOne(e => e.Personality);
            modelBuilder.Entity<Character>().HasOne(e => e.Origin);
            modelBuilder.Entity<Character>().HasOne(e => e.Appearance);

            modelBuilder.Entity<Image>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Image>().Property(p => p.IsAvatar).HasColumnType("bit");
            modelBuilder.Entity<Image>().Property(p => p.ImageData).HasColumnType("LONGBLOB");
        }
    }
}
