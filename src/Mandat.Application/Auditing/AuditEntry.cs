namespace Mandat.Application.Auditing;

public sealed record AuditEntry(
    Guid Id,
    string Actor,
    string Action,
    string ResourceType,
    string ResourceId,
    DateTimeOffset OccurredAt,
    IReadOnlyDictionary<string, string?> Changes);
