using System;
using System.Linq;
using System.Reflection;

namespace CachingDynamicDecorator.Infrastructure
{
    public sealed class MethodInvocation : IComparable
    {
        public MethodInfo MethodInfo { get; }
        public object[] Args { get; }

        public MethodInvocation(MethodInfo methodInfo, object[] args)
        {
            MethodInfo = methodInfo;
            Args = args;
        }

        public override bool Equals(object obj)
        {
            return obj is MethodInvocation identifier 
                && AreMethodEquals(MethodInfo, identifier.MethodInfo) 
                && identifier.Args.SequenceEqual(Args);
        }

        public override int GetHashCode() => ToString().GetHashCode();
        public override string ToString() => $"{MethodInfo.DeclaringType?.Name}{MethodInfo.Name}{string.Join("|", Args.Select(a => a.GetHashCode()))}";
        public int CompareTo(object obj) => string.Compare(ToString(), obj.ToString(), StringComparison.InvariantCultureIgnoreCase);

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
