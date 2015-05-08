namespace CachingDynamicDecorator.UnitTests.TestObjects
{
    public interface IMultipleParameterProvider
    {
        string Provide(string param1, bool param2, FakeReferenceObject param3);
    }
}