using System;
using Castle.DynamicProxy;

namespace CachingDynamicDecorator.CastleObjects
{
    public class CastleCachingDynamicDecorator
    {
        private static readonly ProxyGenerator _generator = new ProxyGenerator();

        public T AddCaching<T>(object target, TimeSpan cacheDuration)
        {
            GuardAgainstInvalidInput<T>(target);
            return (T)_generator.CreateInterfaceProxyWithTarget(
                typeof(T), target, new CastleCachingInterceptor(cacheDuration));
        }

        private static void GuardAgainstInvalidInput<T>(object target)
        {
            if (target == null) 
                throw new ArgumentNullException("target");
            if (!(target is T))
                throw new InvalidOperationException(string.Format("Invalid input object type. Object must be of Type: {0}", typeof(T)));
        }
    }
}