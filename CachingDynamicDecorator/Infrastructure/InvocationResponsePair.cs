namespace CachingDynamicDecorator.Infrastructure
{
    public sealed class InvocationResponsePair
    {
        public MethodIdentifier MethodId { get; }
        public object Response { get; }

        public InvocationResponsePair(MethodIdentifier methodId, object response)
        {
            MethodId = methodId;
            Response = response;
        }

        public bool MatchesId(MethodIdentifier methodId)
        {
            return methodId.Equals(MethodId);
        }
    }
}
