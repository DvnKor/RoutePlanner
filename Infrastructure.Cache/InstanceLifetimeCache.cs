using System;
using System.Collections.Generic;

namespace Infrastructure.Cache
{
    public class InstanceLifetimeCache<TKey, TValue>
    {
        private readonly object _locker = new object();
        private readonly IDictionary<TKey, TValue> _cache = new Dictionary<TKey, TValue>();

        private readonly Func<TKey, TValue> _valueProvider;

        public InstanceLifetimeCache(
            Func<TKey, TValue> valueProvider)
        {
            _valueProvider = valueProvider;
        }

        public TValue Get(TKey key)
        {
            if (!_cache.TryGetValue(key, out var value))
            {
                lock (_locker)
                {
                    if (!_cache.TryGetValue(key, out value))
                    {
                        value = _valueProvider(key);
                        _cache.Add(key, value);
                    }
                }
            }

            return value;
        }
    }
}