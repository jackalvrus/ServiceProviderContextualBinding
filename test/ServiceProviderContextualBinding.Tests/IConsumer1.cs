using System;

namespace ServiceProviderContextualBinding.Tests
{
    public interface IConsumer1
    {
        Type ServiceType1 { get; }
        Type ServiceType2 { get; }
        Type ServiceType3 { get; }
    }
}