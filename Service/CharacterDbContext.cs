using Microsoft.EntityFrameworkCore;
using Service.Models;

namespace Service
{
    public class CharacterDbContext  : DbContext
    {
        public DbSet<Character> Characters { get; set; }
        public DbSet<Origin> Origins { get; set; }
        public DbSet<Breed> Breeds { get; set; }
        public DbSet<Personality> Pertonalities { get; set; }
        public DbSet<Appearance> Appearances { get; set; }
        public DbSet<TypeOfCharacter> TypeOfCharacters { get; set; }
        public DbSet<Orientation> Orientations { get; set; }
        public DbSet<AlignmentChart> AlignmentCharts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=remotemysql.com;database=3axydb5cVN;user=3axydb5cVN;password=KjI6Lektih");
        }

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

        }
    }
}
