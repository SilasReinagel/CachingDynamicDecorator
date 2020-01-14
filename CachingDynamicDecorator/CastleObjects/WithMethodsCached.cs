using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CachingDynamicDecorator.Infrastructure;
using Castle.DynamicProxy;

namespace CachingDynamicDecorator.CastleObjects
{
    public sealed class WithMethodsCached : CastleAsyncInterceptor
    {
        private readonly List<MethodInfo> _targetMethods;
        private readonly TimeSpan _duration;
        private readonly Func<MethodInvocation, string> _getCacheKey;
        private readonly ConcurrentDictionary<string, CachedItem<object>> _cache = new ConcurrentDictionary<string, CachedItem<object>>();

        public WithMethodsCached(Type targetType, TimeSpan duration)
            : this(targetType.GetMethods(BindingFlags.Instance | BindingFlags.Public).ToList(), duration) {}

        public WithMethodsCached(List<MethodInfo> targetMethods, TimeSpan duration)
            : this(targetMethods, duration, m => m.ToString()) {}
        
        public WithMethodsCached(List<MethodInfo> targetMethods, TimeSpan duration, Func<MethodInvocation, string> getCacheKey)
            : base(typeof(WithMethodsCached))
        {
            _targetMethods = targetMethods;
            _duration = duration;
            _getCacheKey = getCacheKey;
        }

        public override void Intercept(IInvocation invocation)
        {
            if (!_targetMethods.Contains(invocation.Method))
                base.Intercept(invocation);
            else
                InvokeAndCacheInvocation(invocation);
        }

        private void InvokeAndCacheInvocation(IInvocation invocation)
        {
            var methodInvocationDetails = new MethodInvocation(invocation.Method, invocation.Arguments);
            var cacheKey = _getCacheKey(methodInvocationDetails);
            
            if (_cache.TryGetValue(cacheKey, out var cached) && !cached.IsExpired)
            {
                invocation.ReturnValue = cached.Item;
                return;
            }

            base.Intercept(invocation);
            _cache.TryAdd(cacheKey, new CachedItem<object>(invocation.ReturnValue, _duration));
        }
    }
}
