using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace CachingDynamicDecorator.UnitTests.TestObjects
{
    [ExcludeFromCodeCoverage]
    public class FakeAsyncProvider : IAsyncProvider
    {
        public readonly List<string> CallsMadeOnProvide1 = new List<string>();
        public readonly List<string> CallsMadeOnProvide2 = new List<string>();

        public Task<string> Provide1(string param)
        {
            CallsMadeOnProvide1.Add(param);
            return Task.FromResult("param" + "-provided");
        }

        public Task<string> Provide2(string param)
        {
            CallsMadeOnProvide2.Add(param);
            return Task.FromResult("param" + "-provided2");
        }
    }
}