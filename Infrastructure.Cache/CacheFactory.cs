using System;

namespace Infrastructure.Cache
{
    public static class CacheFactory
    {
        public static ExpiringCache<TKey, TValue> CreateExpiringCache<TKey, TValue>(
            Func<TKey, TValue> valueProvider, TimeSpan absoluteExpirationRelativeToNow)
        {
            return new (valueProvider, absoluteExpirationRelativeToNow);
        }
    }
}