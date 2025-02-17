using Mandat.Application.Abstractions;
using Mandat.Domain.Entities;

namespace Mandat.Application.Services;

public sealed class MatchRequestService(IMentorRepository mentors, TimeProvider clock)
{
    private static readonly TimeSpan MinimumNotice = TimeSpan.FromDays(2);

    public async Task<MatchRequest> RequestAsync(Guid studentId, Guid mentorId, CancellationToken ct = default)
    {
        var mentor = await mentors.GetByIdAsync(mentorId, ct)
            ?? throw new KeyNotFoundException($"Mentor {mentorId} was not found.");

        if (!mentor.IsActive)
            throw new InvalidOperationException("That mentor is no longer accepting students.");

        var request = new MatchRequest { StudentId = studentId, MentorId = mentorId };
        await mentors.SaveChangesAsync(ct);
        return request;
    }

    public bool CanBookSlot(DateTimeOffset slotStart)
        => slotStart - clock.GetUtcNow() >= MinimumNotice;
}
