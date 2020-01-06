using System.Linq;
using System.Reflection;

namespace CachingDynamicDecorator.Infrastructure
{
    public sealed class MethodIdentifier
    {
        private MethodInfo MethodInfo { get; }
        private object[] Args { get; }

        public MethodIdentifier(MethodInfo methodInfo, object[] args)
        {
            MethodInfo = methodInfo;
            Args = args;
        }

        public override bool Equals(object obj)
        {
            return obj is MethodIdentifier 
                && AreMethodEquals(MethodInfo, ((MethodIdentifier)obj).MethodInfo) 
                && ((MethodIdentifier)obj).Args.SequenceEqual(Args);
        }

        public override int GetHashCode()
        {
            return new { MethodInfo, Args }.GetHashCode();
        }

        private static bool AreMethodEquals(MethodInfo left, MethodInfo right)
        {
            if (left.Equals(right))
                return true;
            if (left.GetHashCode() != right.GetHashCode())
                return false;
            if (left.DeclaringType != right.DeclaringType)
                return false;
            return AreParametersSame(left, right);
        }

        private static bool AreParametersSame(MethodInfo left, MethodInfo right)
        {
            var leftParams = left.GetParameters();
            var rightParams = right.GetParameters();
            if (leftParams.Length != rightParams.Length)
                return false;
            if (leftParams.Where((x, y) => x.ParameterType != rightParams[y].ParameterType).Any())
                return false;
            if (left.ReturnType != right.ReturnType)
                return false;
            return true;
        }
    }
}
