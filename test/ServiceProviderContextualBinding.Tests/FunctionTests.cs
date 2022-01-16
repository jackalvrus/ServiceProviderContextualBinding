using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;

namespace ServiceProviderContextualBinding.Tests
{
    public class FunctionTests
    {
        private IServiceCollection _services;

        [SetUp]
        public void Setup()
        {
            _services = new ServiceCollection();
            _services.AddSingleton<IService1, DefaultService1>();
            _services.AddSingleton<IService2, DefaultService2>();
            _services.AddSingleton<IService3, DefaultService3>();
            _services.AddSingleton<ReplacementService1>();
            _services.AddSingleton<ReplacementService2>();
            _services.AddSingleton<ReplacementService3>();
        }

        [Test]
        public void OneReplacement()
        {
            _services.WithReplacement(typeof(ReplacementService1))
                .Add(typeof(IConsumer1), typeof(Consumer1), ServiceLifetime.Transient);
            var provider = _services.BuildServiceProvider();

            var consumer = provider.GetService<IConsumer1>();

            consumer.Should().NotBeNull();
            consumer.ServiceType1.Should().Be<ReplacementService1>();
            consumer.ServiceType2.Should().Be<DefaultService2>();
            consumer.ServiceType3.Should().Be<DefaultService3>();
        }

        [Test]
        public void TwoReplacements()
        {
            _services.WithReplacement(typeof(ReplacementService1))
                .WithReplacement(typeof(ReplacementService2))
                .Add(typeof(IConsumer1), typeof(Consumer1), ServiceLifetime.Transient);
            var provider = _services.BuildServiceProvider();

            var consumer = provider.GetService<IConsumer1>();

            consumer.Should().NotBeNull();
            consumer.ServiceType1.Should().Be<ReplacementService1>();
            consumer.ServiceType2.Should().Be<ReplacementService2>();
            consumer.ServiceType3.Should().Be<DefaultService3>();
        }

        [Test]
        public void TwoConsumers()
        {
            _services.WithReplacement(typeof(ReplacementService1))
                .Add(typeof(IConsumer1), typeof(Consumer1), ServiceLifetime.Transient)
                .Add(typeof(IConsumer2), typeof(Consumer2), ServiceLifetime.Transient);
            var provider = _services.BuildServiceProvider();

            var consumer1 = provider.GetService<IConsumer1>();
            var consumer2 = provider.GetService<IConsumer2>();

            consumer1.Should().NotBeNull();
            consumer1.ServiceType1.Should().Be<ReplacementService1>();
            consumer1.ServiceType2.Should().Be<DefaultService2>();
            consumer1.ServiceType3.Should().Be<DefaultService3>();
            consumer2.Should().NotBeNull();
            consumer2.ServiceType1.Should().Be<ReplacementService1>();
            consumer2.ServiceType2.Should().Be<DefaultService2>();
            consumer2.ServiceType3.Should().Be<DefaultService3>();
        }

        [Test]
        public void TwoConsumersWithInterleavedReplacement()
        {
            _services.WithReplacement(typeof(ReplacementService1))
                .Add(typeof(IConsumer1), typeof(Consumer1), ServiceLifetime.Transient)
                .WithReplacement(typeof(ReplacementService2))
                .Add(typeof(IConsumer2), typeof(Consumer2), ServiceLifetime.Transient);
            var provider = _services.BuildServiceProvider();

            var consumer1 = provider.GetService<IConsumer1>();
            var consumer2 = provider.GetService<IConsumer2>();

            consumer1.Should().NotBeNull();
            consumer1.ServiceType1.Should().Be<ReplacementService1>();
            consumer1.ServiceType2.Should().Be<DefaultService2>();
            consumer1.ServiceType3.Should().Be<DefaultService3>();
            consumer2.Should().NotBeNull();
            consumer2.ServiceType1.Should().Be<ReplacementService1>();
            consumer2.ServiceType2.Should().Be<ReplacementService2>();
            consumer2.ServiceType3.Should().Be<DefaultService3>();
        }

        [Test]
        public void ExceptionWhenReplacementNotRegistered()
        {
            _services.WithReplacement(typeof(ReplacementServiceNA))
                .Add(typeof(IConsumer1), typeof(Consumer1), ServiceLifetime.Transient);
            var provider = _services.BuildServiceProvider();

            var act = () => provider.GetService<IConsumer1>();

            act.Should().Throw<InvalidOperationException>();
        }

        [Test]
        public void MarkerInterface()
        {
            _services.AddSingleton<IService1Marker, ReplacementServiceWithMarker>();
            _services.WithReplacement<IService1, IService1Marker>()
                .Add(typeof(IConsumer1), typeof(Consumer1), ServiceLifetime.Transient);
            var provider = _services.BuildServiceProvider();

            var consumer = provider.GetService<IConsumer1>();

            consumer.Should().NotBeNull();
            consumer.ServiceType1.Should().Be<ReplacementServiceWithMarker>();
            consumer.ServiceType2.Should().Be<DefaultService2>();
            consumer.ServiceType3.Should().Be<DefaultService3>();
        }

        [Test]
        public void AmbiguousReplacement_AVOID_THIS()
        {
            _services.AddSingleton<ReplacementService1and2>();
            _services.WithReplacement(typeof(ReplacementService1and2))
                .Add(typeof(IConsumer1), typeof(Consumer1), ServiceLifetime.Transient);
            var provider = _services.BuildServiceProvider();

            var consumer = provider.GetService<IConsumer1>();

            consumer.Should().NotBeNull();
            consumer.ServiceType1.Should().Be<ReplacementService1and2>();
            // Even though ReplacementService1_2 matches IService2,
            // it is only used for the one of the constructor arguments.
            consumer.ServiceType2.Should().Be<DefaultService2>();
            consumer.ServiceType3.Should().Be<DefaultService3>();
        }

        [Test]
        public void NullCheck_ReplacementContext_Services()
        {
            IServiceCollection services = null;
            Type type = typeof(ReplacementService1);

            var act = () => new ReplacementContext(services, type);

            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void NullCheck_ReplacementContext_Services_Type()
        {
            IServiceCollection services = new ServiceCollection();
            Type type = null;

            var act = () => new ReplacementContext(services, type);

            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void NullCheck_ReplacementContext_Source_Type()
        {
            ReplacementContext source = new(new ServiceCollection(), typeof(ReplacementService1));
            Type type = null;

            var act = () => new ReplacementContext(source, type);

            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void NullCheck_Extension_Add_ServiceType()
        {
            Type serviceType = null;
            Type implementationType = typeof(Consumer1);

            var act = () => _services.WithReplacement(typeof(ReplacementService1))
                .Add(serviceType, implementationType, ServiceLifetime.Transient);

            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void NullCheck_Extension_Add_ImplementationType()
        {
            Type serviceType = typeof(IConsumer1);
            Type implementationType = null;

            var act = () => _services.WithReplacement(typeof(ReplacementService1))
                .Add(serviceType, implementationType, ServiceLifetime.Transient);

            act.Should().Throw<ArgumentNullException>();
        }
    }
}