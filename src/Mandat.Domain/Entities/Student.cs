namespace Mandat.Domain.Entities;

public sealed class Student
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string DisplayName { get; set; }
    public required string Email { get; set; }
    public Address? Address { get; set; }
    public int? SchoolYear { get; set; }
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? DeletedAt { get; set; }

    public ICollection<MatchRequest> MatchRequests { get; init; } = new List<MatchRequest>();

    public bool IsActive => DeletedAt is null;
}
