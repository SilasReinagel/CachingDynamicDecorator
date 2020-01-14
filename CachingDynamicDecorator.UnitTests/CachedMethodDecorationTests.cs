using System;
using System.Threading.Tasks;
using CachingDynamicDecorator.UnitTests.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CachingDynamicDecorator.UnitTests
{
    [TestClass]
    public sealed class CachedMethodDecorationTests
    {
        private readonly FakeSimpleProvider _fakeSimpleProvider = new FakeSimpleProvider();
        private readonly FakeAsyncProvider _fakeAsyncProvider = new FakeAsyncProvider();

        private readonly TimeSpan _cacheDuration = TimeSpan.FromMilliseconds(5);
        private readonly TimeSpan _expirationDuration = TimeSpan.FromMilliseconds(6);

        private const string _sampleParam1 = "param1";
        private const string _sampleParam2 = "param2";
        
        
        [TestMethod]
        public void CachingDecorator_CacheOnlyOneMethod_OneMethodCachedAndTheOtherIsNot()
        {
            var cached = _fakeSimpleProvider.Cached<ISimpleProvider>(
                _cacheDuration, 
                typeof(ISimpleProvider).GetMethod(nameof(_fakeSimpleProvider.Provide1)),
                m => $"{m.MethodInfo.Name}, {m.Args[0]}");
            
            cached.Provide1(_sampleParam1);
            cached.Provide1(_sampleParam1);
            cached.Provide2(_sampleParam1);
            cached.Provide2(_sampleParam1);

            Assert.AreEqual(1, _fakeSimpleProvider.CallsMadeOnProvide1.Count);
            Assert.AreEqual(2, _fakeSimpleProvider.CallsMadeOnProvide2.Count);
        }
        
        [TestMethod]
        public async Task CachingDecorator_AsyncCacheOnlyOneMethod_OneMethodCachedAndTheOtherIsNot()
        {
            var cached = _fakeAsyncProvider.Cached<IAsyncProvider>(
                _cacheDuration, 
                typeof(IAsyncProvider).GetMethod(nameof(_fakeAsyncProvider.Provide1)),
                m => $"{m.MethodInfo.Name}, {m.Args[0]}");
            
            await cached.Provide1(_sampleParam1);
            await cached.Provide1(_sampleParam1);
            await cached.Provide2(_sampleParam1);
            await cached.Provide2(_sampleParam1);

            Assert.AreEqual(1, _fakeAsyncProvider.CallsMadeOnProvide1.Count);
            Assert.AreEqual(2, _fakeAsyncProvider.CallsMadeOnProvide2.Count);
        }
    }
}
