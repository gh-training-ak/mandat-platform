namespace Mandat.Domain.Entities;

public sealed class ScheduleSlot
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid MentorId { get; init; }
    public DayOfWeek Day { get; set; }
    public TimeOnly StartsAt { get; set; }
    public TimeOnly EndsAt { get; set; }
    public bool IsRecurring { get; set; }

    public TimeSpan Duration => EndsAt - StartsAt;

    public bool Overlaps(ScheduleSlot other)
        => Day == other.Day && StartsAt < other.EndsAt && other.StartsAt < EndsAt;
}
