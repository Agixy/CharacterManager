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
        public DbSet<Relationship> Relationships { get; set; }
        public DbSet<Appearance> Appearances { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<TypeOfCharacter> TypeOfCharacters { get; set; }
        public DbSet<Orientation> Orientations { get; set; }
        public DbSet<AlignmentChart> AlignmentCharts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
         
            modelBuilder.Entity<Character>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Appearance>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Personality>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Origin>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<AlignmentChart>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Relationship>().Property(e => e.Id).ValueGeneratedOnAdd();


            modelBuilder.Entity<Character>().HasOne(e => e.Personality);
            modelBuilder.Entity<Character>().HasOne(e => e.Origin);
            modelBuilder.Entity<Character>().HasOne(e => e.Appearance);

            modelBuilder.Entity<Character>().HasMany(bc => bc.Relationships).WithOne(b => b.Character)
                .HasForeignKey(bc => bc.CharacterId);

            modelBuilder.Entity<BookOrigin>()
                      .HasKey(bc => new { bc.BookId, bc.OriginId });
            modelBuilder.Entity<BookOrigin>()
                .HasOne(bc => bc.Book)
                .WithMany(b => b.BookOrigins)
                .HasForeignKey(bc => bc.BookId);
            modelBuilder.Entity<BookOrigin>()
                .HasOne(bc => bc.Origin)
                .WithMany(c => c.BookOrigins)
                .HasForeignKey(bc => bc.OriginId);
            
        }
    }
}
