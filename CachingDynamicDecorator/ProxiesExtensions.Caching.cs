using System;
using System.Collections.Generic;
using System.Reflection;
using CachingDynamicDecorator.CastleObjects;
using CachingDynamicDecorator.Infrastructure;
using Castle.DynamicProxy;

namespace CachingDynamicDecorator
{
    public static class ProxiesExtensions_Caching
    {
        public static TInterface Cached<TInterface>(this TInterface inner, TimeSpan duration)
            => inner.CreateInterfaceProxyWithTarget(new WithMethodsCached(typeof(TInterface), duration));
        
        public static TInterface Cached<TInterface>(this TInterface inner, TimeSpan duration, MethodInfo mi)
            => inner.CreateInterfaceProxyWithTarget(new WithMethodsCached(new List<MethodInfo> {mi}, duration));
        
        public static TInterface Cached<TInterface>(this TInterface inner, TimeSpan duration, MethodInfo mi, Func<MethodInvocation, string> getCacheKey)
            => inner.CreateInterfaceProxyWithTarget(new WithMethodsCached(new List<MethodInfo> {mi}, duration, getCacheKey));

        private static TInterface CreateInterfaceProxyWithTarget<TInterface>(this TInterface obj, params IInterceptor[] interceptors)
            => (TInterface) new ProxyGenerator().CreateInterfaceProxyWithTarget(typeof(TInterface), obj.EnsureNotNull<TInterface>(), interceptors);

        
        private static T EnsureNotNull<T>(this object target)
        {
            if (target == null) 
                throw new ArgumentNullException(nameof(target));
            if (!(target is T))
                throw new InvalidOperationException($"Invalid input object type. Object must be of Type: {typeof(T)}");
            return (T)target;
        }
    }
}
