using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using CachingDynamicDecorator.UnitTests.Assertions;
using CachingDynamicDecorator.UnitTests.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CachingDynamicDecorator.UnitTests
{
    [TestClass, ExcludeFromCodeCoverage]
    public class DefaultCachingDecoratorTests
    {
        private ISimpleProvider _cachedSimpleProvider;
        private FakeSimpleProvider _fakeSimpleProvider;
        private IAsyncProvider _cachedAsyncProvider;
        private FakeAsyncProvider _fakeAsyncProvider;
        private IReferenceObjectProvider _cachedReferenceObjectProvider;
        private FakeReferenceObjectProvider _fakeReferenceObjectProvider;
        private IMultipleParameterProvider _cachedMultiProvider;
        private FakeMultipleParameterProvider _fakeMultiProvider;

        private DefaultCachingDecorator _dynamicDecorator;

        private readonly TimeSpan _cacheDuration = TimeSpan.FromMilliseconds(5);
        private readonly TimeSpan _expirationDuration = TimeSpan.FromMilliseconds(6);

        private const string _sampleParam1 = "param1";
        private const string _sampleParam2 = "param2";

        [TestInitialize]
        public void Init()
        {
            _fakeSimpleProvider = new FakeSimpleProvider();
            _fakeAsyncProvider = new FakeAsyncProvider();
            _fakeReferenceObjectProvider = new FakeReferenceObjectProvider();
            _fakeMultiProvider = new FakeMultipleParameterProvider();
            _dynamicDecorator = new DefaultCachingDecorator();
            _cachedSimpleProvider = _dynamicDecorator.AddCaching<ISimpleProvider>(_fakeSimpleProvider, _cacheDuration);
            _cachedAsyncProvider = _dynamicDecorator.AddCaching<IAsyncProvider>(_fakeAsyncProvider, _cacheDuration);
            _cachedReferenceObjectProvider = _dynamicDecorator.AddCaching<IReferenceObjectProvider>(_fakeReferenceObjectProvider, _cacheDuration);
            _cachedMultiProvider = _dynamicDecorator.AddCaching<IMultipleParameterProvider>(_fakeMultiProvider, _cacheDuration);
        }

        [TestMethod, TestCategory("Unit Test")]
        public void CachingDecorator_DecorateNullObject_ThrowsArgumentNullException()
        {
            ExceptionAssert.Throws<ArgumentNullException>(() => _dynamicDecorator.AddCaching<ISimpleProvider>(null, _cacheDuration));
        }

        [TestMethod, TestCategory("Unit Test")]
        public void CachingDecorator_DecorateObjectOfWrongType_ThrowsInvalidOperationException()
        {
            ExceptionAssert.Throws<InvalidOperationException>(() => _dynamicDecorator.AddCaching<ISimpleProvider>("invalidObjectType", _cacheDuration));
        }

        [TestMethod, TestCategory("Unit Test")]
        public void CachingDecorator_SingleCallToDecoratedMethod_MethodWasCalledWithCorrectParameter()
        {
            _cachedSimpleProvider.Provide1(_sampleParam1);

            Assert.AreEqual(1, _fakeSimpleProvider.CallsMadeOnProvide1.Count);
            Assert.AreEqual(true, _fakeSimpleProvider.CallsMadeOnProvide1.Contains(_sampleParam1));
        }

        [TestMethod, TestCategory("Unit Test")]
        public void CachingDecorator_TwoIdenticalCallsToDecoratedMethodInQuickSuccession_MethodWasCalledOnlyOnce()
        {
            _cachedSimpleProvider.Provide1(_sampleParam1);
            _cachedSimpleProvider.Provide1(_sampleParam1);

            Assert.AreEqual(1, _fakeSimpleProvider.CallsMadeOnProvide1.Count);
        }

        [TestMethod, TestCategory("Unit Test")]
        public void CachingDecorator_DifferentInputCallsToDecoratedMethod_MethodWasCalledTwice()
        {
            _cachedSimpleProvider.Provide1(_sampleParam1);
            _cachedSimpleProvider.Provide1(_sampleParam2);

            Assert.AreEqual(2, _fakeSimpleProvider.CallsMadeOnProvide1.Count);
        }

        [TestMethod, TestCategory("Unit Test")]
        public void CachingDecorator_TwoIdenticalCallsToDecoratedMethodWithDelayBetween_MethodWasCalledTwice()
        {
            _cachedSimpleProvider.Provide1(_sampleParam1);
            Thread.Sleep(_expirationDuration);
            _cachedSimpleProvider.Provide1(_sampleParam1);

            Assert.AreEqual(2, _fakeSimpleProvider.CallsMadeOnProvide1.Count);
        }

        [TestMethod, TestCategory("Unit Test")]
        public void CachingDecorator_CallsMadeToDifferentDecoratedMethodsWithSameInputsInQuickSuccession_EachMethodWasCalledOnce()
        {
            _cachedSimpleProvider.Provide1(_sampleParam1);
            _cachedSimpleProvider.Provide2(_sampleParam1);

            Assert.AreEqual(1, _fakeSimpleProvider.CallsMadeOnProvide1.Count);
            Assert.AreEqual(1, _fakeSimpleProvider.CallsMadeOnProvide2.Count);
        }

        [TestMethod, TestCategory("Unit Test")]
        public async Task CachingDecorator_SingleCallToAsyncDecoratedMethod_MethodWasCalledWithCorrectParameter()
        {
            await _cachedAsyncProvider.Provide1(_sampleParam1);

            Assert.AreEqual(1, _fakeAsyncProvider.CallsMadeOnProvide1.Count);
            Assert.AreEqual(true, _fakeAsyncProvider.CallsMadeOnProvide1.Contains(_sampleParam1));
        }

        [TestMethod, TestCategory("Unit Test")]
        public async Task CachingDecorator_TwoIdenticalCallsToAsyncDecoratedMethodInQuickSuccession_MethodWasCalledOnlyOnce()
        {
            await _cachedAsyncProvider.Provide1(_sampleParam1);
            await _cachedAsyncProvider.Provide1(_sampleParam1);

            Assert.AreEqual(1, _fakeAsyncProvider.CallsMadeOnProvide1.Count);
        }

        [TestMethod, TestCategory("Unit Test")]
        public async Task CachingDecorator_DifferentInputCallsToAsyncDecoratedMethod_MethodWasCalledTwice()
        {
            await _cachedAsyncProvider.Provide1(_sampleParam1);
            await _cachedAsyncProvider.Provide1(_sampleParam2);

            Assert.AreEqual(2, _fakeAsyncProvider.CallsMadeOnProvide1.Count);
        }

        [TestMethod, TestCategory("Unit Test")]
        public async Task CachingDecorator_TwoIdenticalCallsToAsyncDecoratedMethodWithDelayBetween_MethodWasCalledTwice()
        {
            await _cachedAsyncProvider.Provide1(_sampleParam1);
            await Task.Delay(_expirationDuration);
            await _cachedAsyncProvider.Provide1(_sampleParam1);

            Assert.AreEqual(2, _fakeAsyncProvider.CallsMadeOnProvide1.Count);
        }

        [TestMethod, TestCategory("Unit Test")]
        public async Task CachingDecorator_CallsMadeToDifferentAsyncDecoratedMethodsWithSameInputsInQuickSuccession_EachMethodWasCalledOnce()
        {
            await _cachedAsyncProvider.Provide1(_sampleParam1);
            await _cachedAsyncProvider.Provide2(_sampleParam1);

            Assert.AreEqual(1, _fakeAsyncProvider.CallsMadeOnProvide1.Count);
            Assert.AreEqual(1, _fakeAsyncProvider.CallsMadeOnProvide2.Count);
        }

        [TestMethod, TestCategory("Unit Test")]
        public void CachingDecorator_TwoIdenticalCallsToReferenceObjectMethodInQuickSuccession_MethodWasCalledOnlyOnce()
        {
            var refObj = new FakeReferenceObject();
            _cachedReferenceObjectProvider.Provide(refObj);
            _cachedReferenceObjectProvider.Provide(refObj);

            Assert.AreEqual(1, _fakeReferenceObjectProvider.CallsMadeOnProvide.Count);
        }

        [TestMethod, TestCategory("Unit Test")]
        public void CachingDecorator_DifferentInputCallsToReferenceObjectMethod_MethodWasCalledTwice()
        {
            _cachedReferenceObjectProvider.Provide(new FakeReferenceObject());
            _cachedReferenceObjectProvider.Provide(new FakeReferenceObject());

            Assert.AreEqual(2, _fakeReferenceObjectProvider.CallsMadeOnProvide.Count);
        }

        [TestMethod, TestCategory("Unit Test")]
        public void CachingDecorator_TwoIdenticalCallsToReferenceObjectMethodWithDelayBetween_MethodWasCalledTwice()
        {
            var refObj = new FakeReferenceObject();
            _cachedReferenceObjectProvider.Provide(refObj);
            Thread.Sleep(_expirationDuration);
            _cachedReferenceObjectProvider.Provide(refObj);

            Assert.AreEqual(2, _fakeReferenceObjectProvider.CallsMadeOnProvide.Count);
        }

        [TestMethod, TestCategory("Unit Test")]
        public void CachingDecorator_TwoIdenticalCallsToMultiParamMethodInQuickSuccession_MethodWasCalledOnlyOnce()
        {
            var refObj = new FakeReferenceObject();
            _cachedMultiProvider.Provide(_sampleParam1, true, refObj);
            _cachedMultiProvider.Provide(_sampleParam1, true, refObj);

            Assert.AreEqual(1, _fakeMultiProvider.CallsMadeOnProvide.Count);
        }

        [TestMethod, TestCategory("Unit Test")]
        public void CachingDecorator_DifferentInputCallsToMultiParamMethod_MethodWasCalledTheCorrectNumberOfTimes()
        {
            var refObj1 = new FakeReferenceObject();
            var refObj2 = new FakeReferenceObject();
            _cachedMultiProvider.Provide(_sampleParam1, true, refObj1);
            _cachedMultiProvider.Provide(_sampleParam1, true, refObj2);
            _cachedMultiProvider.Provide(_sampleParam1, false, refObj1);
            _cachedMultiProvider.Provide(_sampleParam1, false, refObj2);
            _cachedMultiProvider.Provide(_sampleParam2, true, refObj1);
            _cachedMultiProvider.Provide(_sampleParam2, true, refObj2);
            _cachedMultiProvider.Provide(_sampleParam2, false, refObj1);
            _cachedMultiProvider.Provide(_sampleParam2, false, refObj2);

            Assert.AreEqual(8, _fakeMultiProvider.CallsMadeOnProvide.Count);
        }
    }
}