using Mandat.Domain.Entities;

namespace Mandat.Application.Abstractions;

public sealed record MentorSearchCriteria
{
    public Subject? Subject { get; init; }
    public MeetingType? MeetingType { get; init; }
    public decimal? MaxHourlyRate { get; init; }
    public decimal? MinimumRating { get; init; }
    public Address? Near { get; init; }
    public double? WithinKm { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}
