using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CachingDynamicDecorator.UnitTests.TestObjects
{
    [ExcludeFromCodeCoverage]
    public class FakeMultipleParameterProvider : IMultipleParameterProvider
    {
        public List<Tuple<string, bool, FakeReferenceObject>> CallsMadeOnProvide = new List<Tuple<string, bool, FakeReferenceObject>>(); 

        public string Provide(string param1, bool param2, FakeReferenceObject param3)
        {
            CallsMadeOnProvide.Add(new Tuple<string, bool, FakeReferenceObject>(param1, param2, param3));
            return "provided";
        }
    }
}