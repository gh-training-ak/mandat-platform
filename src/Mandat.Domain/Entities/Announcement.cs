namespace Mandat.Domain.Entities;

public sealed class Announcement
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid MentorId { get; init; }
    public required string Title { get; set; }
    public required string Body { get; set; }
    public Subject Subject { get; set; }
    public MeetingType MeetingType { get; set; }
    public decimal PricePerHour { get; set; }
    public bool IsPublished { get; set; }
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
}
