﻿using System;

namespace ServiceProviderContextualBinding.Tests
{
    public class Consumer2 : IConsumer2
    {
        public Consumer2(
            IService1 service1,
            IService2 service2,
            IService3 service3)
        {
            ServiceType1 = service1.GetType();
            ServiceType2 = service2.GetType();
            ServiceType3 = service3.GetType();
        }

        public Type ServiceType1 { get; }
        public Type ServiceType2 { get; }
        public Type ServiceType3 { get; }
    }
}
