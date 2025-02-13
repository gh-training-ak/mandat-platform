using Mandat.Application.Abstractions;
using Mandat.Domain.Entities;

namespace Mandat.Application.Services;

public sealed class MentorSearchService(IMentorRepository repository)
{
    public async Task<IReadOnlyList<MentorSearchResult>> SearchAsync(
        MentorSearchCriteria criteria,
        CancellationToken ct = default)
    {
        if (criteria.PageSize is < 1 or > 100)
            throw new ArgumentOutOfRangeException(nameof(criteria), "PageSize must be between 1 and 100.");

        var mentors = await repository.SearchAsync(criteria, ct);

        return mentors
            .Where(m => m.IsActive)
            .Select(m => new MentorSearchResult(
                m.Id,
                m.DisplayName,
                m.HourlyRate,
                m.AverageRating,
                DistanceFor(m, criteria.Near)))
            .OrderByDescending(r => r.Rating)
            .ThenBy(r => r.DistanceKm ?? double.MaxValue)
            .ToList();
    }

    private static double? DistanceFor(Mentor mentor, Address? origin)
        => mentor.Address is null || origin is null ? null : origin.DistanceKmTo(mentor.Address);
}

public sealed record MentorSearchResult(
    Guid Id,
    string DisplayName,
    decimal HourlyRate,
    decimal Rating,
    double? DistanceKm);
