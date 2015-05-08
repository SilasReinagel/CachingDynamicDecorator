namespace CachingDynamicDecorator.Infrastructure
{
    public class InvocationResponsePair
    {
        public MethodIdentifier MethodId { get; private set; }
        public object Response { get; private set; }

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