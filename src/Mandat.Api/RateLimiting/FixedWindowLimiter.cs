// first attempt

namespace Mandat.Api.RateLimiting;

public sealed class FixedWindowLimiter(RateLimitOptions options)
{
    private readonly Dictionary<string, (int Count, DateTimeOffset WindowStart)> _buckets = new();

    public bool TryAcquire(string clientId, DateTimeOffset now)
    {
        if (!_buckets.TryGetValue(clientId, out var bucket) || now - bucket.WindowStart > options.Window)
        {
            _buckets[clientId] = (1, now);
            return true;
        }

        if (bucket.Count >= options.PermitLimit)
            return false;

        _buckets[clientId] = (bucket.Count + 1, bucket.WindowStart);
        return true;
    }
}

public sealed record RateLimitOptions(int PermitLimit, TimeSpan Window);
