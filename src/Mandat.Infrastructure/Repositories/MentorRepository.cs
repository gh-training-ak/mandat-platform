using Mandat.Application.Abstractions;
using Mandat.Domain.Entities;
using Mandat.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Mandat.Infrastructure.Repositories;

public sealed class MentorRepository(MandatDbContext db) : IMentorRepository
{
    public Task<Mentor?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => db.Mentors.Include(m => m.ReviewsReceived).FirstOrDefaultAsync(m => m.Id == id, ct);

    public async Task<IReadOnlyList<Mentor>> SearchAsync(MentorSearchCriteria criteria, CancellationToken ct = default)
    {
        var query = db.Mentors.AsNoTracking().Where(m => m.DeletedAt == null);

        if (criteria.MaxHourlyRate is { } rate)
            query = query.Where(m => m.HourlyRate <= rate);

        if (criteria.MeetingType is MeetingType.Online)
            query = query.Where(m => m.AcceptsOnline);

        if (criteria.MeetingType is MeetingType.InPerson)
            query = query.Where(m => m.AcceptsInPerson);

        return await query
            .OrderBy(m => m.DisplayName)
            .Skip((criteria.Page - 1) * criteria.PageSize)
            .Take(criteria.PageSize)
            .ToListAsync(ct);
    }

    public async Task AddAsync(Mentor mentor, CancellationToken ct = default)
        => await db.Mentors.AddAsync(mentor, ct);

    public Task<int> SaveChangesAsync(CancellationToken ct = default) => db.SaveChangesAsync(ct);
}
