using Mandat.Domain.Entities;

namespace Mandat.Application.Abstractions;

public interface IMentorRepository
{
    Task<Mentor?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Mentor>> SearchAsync(MentorSearchCriteria criteria, CancellationToken ct = default);
    Task AddAsync(Mentor mentor, CancellationToken ct = default);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
