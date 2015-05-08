using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CachingDynamicDecorator.UnitTests.TestObjects
{
    [ExcludeFromCodeCoverage]
    public class FakeReferenceObjectProvider : IReferenceObjectProvider
    {
        public List<FakeReferenceObject> CallsMadeOnProvide = new List<FakeReferenceObject>(); 

        public string Provide(FakeReferenceObject param)
        {
            CallsMadeOnProvide.Add(param);
            return "provided";
        }
    }
}