using System;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace CachingDynamicDecorator.CastleObjects
{
    public abstract class CastleAsyncInterceptor : IInterceptor
    {
        private readonly MethodInfo _handleAsyncMethodInfo;

        protected CastleAsyncInterceptor(Type extendingType)
        {
            _handleAsyncMethodInfo = extendingType.GetMethod("HandleAsyncWithResult", BindingFlags.Instance | BindingFlags.Public);
        }

        public virtual void Intercept(IInvocation invocation)
        {
            var delegateType = GetDelegateType(invocation);
            if (delegateType == MethodType.Synchronous)
            {
                invocation.Proceed();
            }
            if (delegateType == MethodType.AsyncAction)
            {
                invocation.Proceed();
                invocation.ReturnValue = HandleAsync((Task)invocation.ReturnValue);
            }
            if (delegateType == MethodType.AsyncFunction)
            {
                invocation.Proceed();
                ExecuteHandleAsyncWithResultUsingReflection(invocation);
            }
        }

        private void ExecuteHandleAsyncWithResultUsingReflection(IInvocation invocation)
        {
            var resultType = invocation.Method.ReturnType.GetGenericArguments()[0];
            var mi = _handleAsyncMethodInfo.MakeGenericMethod(resultType);
            invocation.ReturnValue = mi.Invoke(this, new[] { invocation.ReturnValue });
        }

        public virtual async Task HandleAsync(Task task)
        {
            await task;
        }

        public virtual async Task<T> HandleAsyncWithResult<T>(Task<T> task)
        {
            return await task;
        }

        private MethodType GetDelegateType(IInvocation invocation)
        {
            var returnType = invocation.Method.ReturnType;
            if (returnType == typeof(Task))
                return MethodType.AsyncAction;
            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
                return MethodType.AsyncFunction;
            return MethodType.Synchronous;
        }

        private enum MethodType
        {
            Synchronous,
            AsyncAction,
            AsyncFunction
        }
    }
}