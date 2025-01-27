namespace Mandat.Domain.Entities;

public sealed class MatchRequest
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid StudentId { get; init; }
    public Guid MentorId { get; init; }
    public MatchStatus Status { get; private set; } = MatchStatus.Pending;
    public DateTimeOffset RequestedAt { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? AnsweredAt { get; private set; }

    public void Accept()
    {
        EnsurePending();
        Status = MatchStatus.Accepted;
        AnsweredAt = DateTimeOffset.UtcNow;
    }

    public void Reject()
    {
        EnsurePending();
        Status = MatchStatus.Rejected;
        AnsweredAt = DateTimeOffset.UtcNow;
    }

    private void EnsurePending()
    {
        if (Status is not MatchStatus.Pending)
            throw new InvalidOperationException($"Request {Id} was already answered with {Status}.");
    }
}
