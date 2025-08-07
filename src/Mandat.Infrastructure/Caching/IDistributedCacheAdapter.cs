namespace Mandat.Infrastructure.Caching;

public interface IDistributedCacheAdapter
{
    Task<string?> GetStringAsync(string key, CancellationToken ct = default);
    Task SetStringAsync(string key, string value, TimeSpan ttl, CancellationToken ct = default);
}
