using Mandat.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mandat.Infrastructure.Persistence;

public sealed class MandatDbContext(DbContextOptions<MandatDbContext> options) : DbContext(options)
{
    public DbSet<Mentor> Mentors => Set<Mentor>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Announcement> Announcements => Set<Announcement>();
    public DbSet<MatchRequest> MatchRequests => Set<MatchRequest>();
    public DbSet<Review> Reviews => Set<Review>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Mentor>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.DisplayName).HasMaxLength(200).IsRequired();
            entity.Property(m => m.Email).HasMaxLength(320).IsRequired();
            entity.Property(m => m.HourlyRate).HasPrecision(10, 2);
            entity.HasIndex(m => m.Email).IsUnique();
            entity.OwnsOne(m => m.Address);
        });

        builder.Entity<MatchRequest>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.HasIndex(r => new { r.StudentId, r.MentorId }).IsUnique();
        });

        builder.Entity<Review>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.ToTable(t => t.HasCheckConstraint("CK_Review_Score", "[Score] BETWEEN 1 AND 5"));
        });
    }
}
