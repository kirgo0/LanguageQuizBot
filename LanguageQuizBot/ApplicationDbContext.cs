using LanguageQuizBot.Models;
using Microsoft.EntityFrameworkCore;

namespace LanguageQuizBot
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Word> Words { get; set; }

        public DbSet<UserWordGrade> UserWordGrades { get; set; }

        public DbSet<Translation> Translations { get; set; }

        public DbSet<Language> Languages { get; set; }

        public DbSet<Lexicon> Lexicons { get; set; }

        public DbSet<LexiconWord> LexiconWords { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // UserWordGrade configuration
            modelBuilder.Entity<UserWordGrade>()
                .HasKey(uwg => new { uwg.UserId, uwg.WordId });

            // LexiconWord configuration
            modelBuilder.Entity<LexiconWord>()
                .HasKey(lw => new { lw.LexiconId, lw.WordId });
        }
    }
}
