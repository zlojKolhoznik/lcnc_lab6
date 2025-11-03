using Microsoft.EntityFrameworkCore;
using OnlineForms.Models;

namespace OnlineForms.Data;

public class OnlineFormsDbContext : DbContext
{
    public OnlineFormsDbContext(DbContextOptions<OnlineFormsDbContext> options) : base(options)
    {
    }

    public DbSet<Survey> Surveys => Set<Survey>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<Response> Responses => Set<Response>();
    public DbSet<ResponseAnswer> ResponseAnswers => Set<ResponseAnswer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Survey>()
            .HasMany(s => s.Questions)
            .WithOne(q => q.Survey!)
            .HasForeignKey(q => q.SurveyId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Survey>()
            .HasMany(s => s.Responses)
            .WithOne(r => r.Survey!)
            .HasForeignKey(r => r.SurveyId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Response>()
            .HasMany(r => r.Answers)
            .WithOne(a => a.Response!)
            .HasForeignKey(a => a.ResponseId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Question>()
            .HasMany(q => q.Answers)
            .WithOne(a => a.Question!)
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        // Enum conversion for SQLite (stores as string for readability)
        modelBuilder.Entity<ResponseAnswer>()
            .Property(a => a.SelectedOption)
            .HasConversion<string>()
            .HasMaxLength(1);

        // Indexes
        modelBuilder.Entity<Response>()
            .HasIndex(r => new { r.SurveyId, r.ParticipantId });

        modelBuilder.Entity<Question>()
            .HasIndex(q => new { q.SurveyId, q.Order });
    }
}
