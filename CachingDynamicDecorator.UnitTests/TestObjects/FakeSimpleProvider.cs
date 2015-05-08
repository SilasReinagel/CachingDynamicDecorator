using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CachingDynamicDecorator.UnitTests.TestObjects
{
    [ExcludeFromCodeCoverage]
    public class FakeSimpleProvider : ISimpleProvider
    {
        public readonly List<string> CallsMadeOnProvide1 = new List<string>();
        public readonly List<string> CallsMadeOnProvide2 = new List<string>();

        public string Provide1(string param)
        {
            CallsMadeOnProvide1.Add(param);
            return "param" + "-provided";
        }

        public string Provide2(string param)
        {
            CallsMadeOnProvide2.Add(param);
            return "param" + "-provided2";
        }
    }
}