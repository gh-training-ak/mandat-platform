namespace Mandat.Domain.Entities;

public sealed class Mentor
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string DisplayName { get; set; }
    public required string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Biography { get; set; }
    public Address? Address { get; set; }
    public decimal HourlyRate { get; set; }
    public bool AcceptsOnline { get; set; } = true;
    public bool AcceptsInPerson { get; set; }
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? DeletedAt { get; set; }

    public ICollection<Announcement> Announcements { get; init; } = new List<Announcement>();
    public ICollection<Review> ReviewsReceived { get; init; } = new List<Review>();

    public bool IsActive => DeletedAt is null;

    public decimal AverageRating =>
        ReviewsReceived.Count == 0 ? 0m : Math.Round(ReviewsReceived.Average(r => r.Score), 2);
}
