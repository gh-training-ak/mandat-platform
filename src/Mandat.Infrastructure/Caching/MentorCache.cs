using System.Text.Json;
using Mandat.Domain.Entities;

namespace Mandat.Infrastructure.Caching;

public sealed class MentorCache(IDistributedCacheAdapter cache)
{
    private static readonly TimeSpan Ttl = TimeSpan.FromSeconds(60);

    public async Task<IReadOnlyList<Mentor>?> TryGetAsync(string key, CancellationToken ct = default)
    {
        var payload = await cache.GetStringAsync(key, ct);
        return payload is null ? null : JsonSerializer.Deserialize<List<Mentor>>(payload);
    }

    public Task SetAsync(string key, IReadOnlyList<Mentor> mentors, CancellationToken ct = default)
        => cache.SetStringAsync(key, JsonSerializer.Serialize(mentors), Ttl, ct);
}
