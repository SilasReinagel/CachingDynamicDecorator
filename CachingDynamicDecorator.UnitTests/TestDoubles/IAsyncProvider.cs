using System.Threading.Tasks;

namespace CachingDynamicDecorator.UnitTests.TestObjects
{
    public interface IAsyncProvider
    {
        Task<string> Provide1(string param);
        Task<string> Provide2(string param);
    }
}