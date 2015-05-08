using System;
using CachingDynamicDecorator.CastleObjects;

namespace CachingDynamicDecorator
{
    public class DefaultCachingDecorator
    {
        private readonly CastleCachingDynamicDecorator _decorator;

        public DefaultCachingDecorator()
        {
            _decorator = new CastleCachingDynamicDecorator();
        }

        public T AddCaching<T>(object target, TimeSpan cacheDuration)
        {
            return _decorator.AddCaching<T>(target, cacheDuration);
        }
    }
}