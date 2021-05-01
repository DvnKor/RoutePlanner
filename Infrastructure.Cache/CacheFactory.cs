using System;

namespace Infrastructure.Cache
{
    public static class CacheFactory
    {
        public static InstanceLifetimeCache<TKey, TValue> CreateInstanceLifetimeCache<TKey, TValue>(
            Func<TKey, TValue> valueProvider)
        {
            return new InstanceLifetimeCache<TKey, TValue>(valueProvider);
        }
        
        public static ExpiringCache<TKey, TValue> CreateExpiringCache<TKey, TValue>(
            Func<TKey, TValue> valueProvider, int cacheExpirationMinutes)
        {
            return new ExpiringCache<TKey, TValue>(valueProvider, cacheExpirationMinutes);
        }
    }
}