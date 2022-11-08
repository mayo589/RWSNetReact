using Microsoft.EntityFrameworkCore;
using TranslationManagement.Api.Enums;
using TranslationManagement.Api.Models;

namespace TranslationManagement.Api
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<TranslationJob> TranslationJobs { get; set; }
        public DbSet<Translator> Translators { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Translator>()
             .Property(t => t.Status)
             .HasDefaultValue(TranslatorStatus.Applicant)
             .HasConversion<string>();

            modelBuilder.Entity<TranslationJob>()
             .Property(j => j.Status)
             .HasDefaultValue(TranslationJobStatus.New)
             .HasConversion<string>();

            modelBuilder.Entity<Translator>()
            .HasMany<TranslationJob>(t => t.TranslationJobs)
            .WithOne(j => j.Translator)
            .HasForeignKey(s => s.TranslatorId);
        }
    }
}