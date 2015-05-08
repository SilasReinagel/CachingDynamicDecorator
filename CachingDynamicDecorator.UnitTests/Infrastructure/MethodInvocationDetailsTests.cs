using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using CachingDynamicDecorator.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CachingDynamicDecorator.UnitTests.Infrastructure
{
    [TestClass, ExcludeFromCodeCoverage]
    public class MethodInvocationDetailsTests
    {
        [TestMethod, TestCategory("Unit Test")]
        public void MethodInvocationDetails_DifferentMethodInputTypes_AreEqualIsFalse()
        {
            var detail1 = new MethodIdentifier(
                GetMethodInfo("SampleMethod", new [] { typeof(bool) }), 
                new object[] { true });
            var detail2 = new MethodIdentifier(
                GetMethodInfo("SampleMethod", new [] { typeof(string) }), 
                new object[] { "input" });

            Assert.AreEqual(false, detail1.Equals(detail2));
        }

        [TestMethod, TestCategory("Unit Test")]
        public void MethodInvocationDetails_DifferentMethodNames_AreEqualIsFalse()
        {
            var detail1 = new MethodIdentifier(
                GetMethodInfo("SampleMethod", new[] { typeof(string) }),
                new object[] { "input" });
            var detail2 = new MethodIdentifier(
                GetMethodInfo("DifferentMethod", new[] { typeof(string) }),
                new object[] { "input" });

            Assert.AreEqual(false, detail1.Equals(detail2));
        }

        [TestMethod, TestCategory("Unit Test")]
        public void MethodInvocationDetails_DifferentInputValues_AreEqualIsFalse()
        {
            var detail1 = new MethodIdentifier(
                GetMethodInfo("SampleMethod", new[] { typeof(string) }),
                new object[] { "input" });
            var detail2 = new MethodIdentifier(
                GetMethodInfo("SampleMethod", new[] { typeof(string) }),
                new object[] { "input2" });

            Assert.AreEqual(false, detail1.Equals(detail2));
        }

        [TestMethod, TestCategory("Unit Test")]
        public void MethodInvocationDetails_CompareIdenticalMethodInvocations_AreEqualIsTrue()
        {
            var detail1 = new MethodIdentifier(
                GetMethodInfo("SampleMethod", new[] { typeof(string) }),
                new object[] { "input" });
            var detail2 = new MethodIdentifier(
                GetMethodInfo("SampleMethod", new[] { typeof(string) }),
                new object[] { "input" });

            Assert.AreEqual(true, detail1.Equals(detail2));
        }

        private MethodInfo GetMethodInfo(string methodName, Type[] inputTypes)
        {
            return GetType().GetMethod(methodName, inputTypes);
        }

        public bool SampleMethod(bool input)
        {
            return true;
        }

        public string SampleMethod(string input)
        {
            return string.Empty;
        }

        public string DifferentMethod(string input)
        {
            return string.Empty;
        }
    }
}