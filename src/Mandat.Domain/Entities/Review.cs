namespace Mandat.Domain.Entities;

public sealed class Review
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid AuthorId { get; init; }
    public Guid SubjectId { get; init; }
    public int Score { get; init; }
    public string? Comment { get; set; }
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
}
