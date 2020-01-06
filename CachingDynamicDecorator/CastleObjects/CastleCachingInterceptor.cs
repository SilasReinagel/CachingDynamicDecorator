using System;
using CachingDynamicDecorator.Infrastructure;
using Castle.DynamicProxy;

namespace CachingDynamicDecorator.CastleObjects
{
    public sealed class CastleCachingInterceptor : CastleAsyncInterceptor
    {
        private readonly CachedInvocationResponseCollection _cache;

        public CastleCachingInterceptor(TimeSpan dataExpirationDuration)
            : base(typeof(CastleCachingInterceptor))
        {
            _cache = new CachedInvocationResponseCollection(dataExpirationDuration);
        }

        public override void Intercept(IInvocation invocation)
        {
            var methodInvocationDetails = new MethodIdentifier(invocation.Method, invocation.Arguments);

            if (_cache.ContainsById(methodInvocationDetails))
            {
                invocation.ReturnValue = _cache.GetValueById(methodInvocationDetails);
                return;
            }

            base.Intercept(invocation);
            _cache.Add(new InvocationResponsePair(methodInvocationDetails, invocation.ReturnValue));
        }
    }
}
