using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CachingDynamicDecorator.UnitTests.Assertions
{
    [ExcludeFromCodeCoverage]
    public static class ExceptionAssert
    {
        public static void Throws<T>(Action action) where T : Exception
        {
            try
            {
                action.Invoke();
                Assert.Fail("Expected exception of type {0}, but no exception was thrown.", typeof(T));
            }
            catch (Exception ex)
            {
                if (!(ex is T))
                    Assert.Fail("Expected exception type: <{0}>. Actual: <{1}>", typeof(T), ex.GetType());
            }
        }
    }
}