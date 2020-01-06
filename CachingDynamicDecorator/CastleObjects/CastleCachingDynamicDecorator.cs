using System;
using Castle.DynamicProxy;

namespace CachingDynamicDecorator.CastleObjects
{
    public sealed class CastleCachingDynamicDecorator
    {
        private static readonly ProxyGenerator Generator = new ProxyGenerator();

        public T AddCaching<T>(object target, TimeSpan cacheDuration)
        {
            GuardAgainstInvalidInput<T>(target);
            return (T)Generator.CreateInterfaceProxyWithTarget(
                typeof(T), target, new CastleCachingInterceptor(cacheDuration));
        }

        private static void GuardAgainstInvalidInput<T>(object target)
        {
            if (target == null) 
                throw new ArgumentNullException(nameof(target));
            if (!(target is T))
                throw new InvalidOperationException($"Invalid input object type. Object must be of Type: {typeof(T)}");
        }
    }
}
