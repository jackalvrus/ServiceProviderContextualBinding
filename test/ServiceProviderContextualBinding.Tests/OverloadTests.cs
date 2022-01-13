using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Linq;

namespace ServiceProviderContextualBinding.Tests
{
    public class OverloadTests
    {
        [Test]
        public void WithReplacement_TService_TReplacement_IServiceCollection()
        {
            var services = new ServiceCollection();

            var context = services.WithReplacement<IService1, ReplacementService1>();

            context.Services.Should().BeSameAs(services);
            context.ReplacementTypes.Should().BeEquivalentTo(new [] { typeof(ReplacementService1) });
        }

        [Test]
        public void WithReplacement_TService_TReplacement_ReplacementContext()
        {
            var services = new ServiceCollection();
            var source = new ReplacementContext(services, typeof(ReplacementService1));

            var context = source.WithReplacement<IService2, ReplacementService2>();

            context.Services.Should().BeSameAs(services);
            context.ReplacementTypes.Should().BeEquivalentTo(new[] { typeof(ReplacementService1), typeof(ReplacementService2) });
        }

        [Test]
        public void WithReplacement_TReplacement_IServiceCollection()
        {
            var services = new ServiceCollection();

            var context = services.WithReplacement<ReplacementService1>();

            context.Services.Should().BeSameAs(services);
            context.ReplacementTypes.Should().BeEquivalentTo(new[] { typeof(ReplacementService1) });
        }

        [Test]
        public void WithReplacement_TReplacement_ReplacementContext()
        {
            var services = new ServiceCollection();
            var source = new ReplacementContext(services, typeof(ReplacementService1));

            var context = source.WithReplacement<ReplacementService2>();

            context.Services.Should().BeSameAs(services);
            context.ReplacementTypes.Should().BeEquivalentTo(new[] { typeof(ReplacementService1), typeof(ReplacementService2) });
        }

        [Test]
        public void WithReplacement_IServiceCollection_Type()
        {
            var services = new ServiceCollection();

            var context = services.WithReplacement(typeof(ReplacementService1));

            context.Services.Should().BeSameAs(services);
            context.ReplacementTypes.Should().BeEquivalentTo(new[] { typeof(ReplacementService1) });
        }

        [Test]
        public void WithReplacement_ReplacementContext_Type()
        {
            var services = new ServiceCollection();
            var source = new ReplacementContext(services, typeof(ReplacementService1));

            var context = source.WithReplacement(typeof(ReplacementService2));

            context.Services.Should().BeSameAs(services);
            context.ReplacementTypes.Should().BeEquivalentTo(new[] { typeof(ReplacementService1), typeof(ReplacementService2) });
        }

        [Test]
        public void AddTransient_TService()
        {
            var services = new ServiceCollection();
            var context = new ReplacementContext(services, typeof(ReplacementService1));

            context.AddTransient<Consumer1>();

            services.Count.Should().Be(1);
            var descriptor = services.First();
            descriptor.Lifetime.Should().Be(ServiceLifetime.Transient);
            descriptor.ServiceType.Should().Be(typeof(Consumer1));
            descriptor.ImplementationType.Should().BeNull();
            descriptor.ImplementationInstance.Should().BeNull();
            descriptor.ImplementationFactory.Should().NotBeNull();
        }

        [Test]
        public void AddTransient_TService_TImplementation()
        {
            var services = new ServiceCollection();
            var context = new ReplacementContext(services, typeof(ReplacementService1));

            context.AddTransient<IConsumer1, Consumer1>();

            services.Count.Should().Be(1);
            var descriptor = services.First();
            descriptor.Lifetime.Should().Be(ServiceLifetime.Transient);
            descriptor.ServiceType.Should().Be(typeof(IConsumer1));
            descriptor.ImplementationType.Should().BeNull();
            descriptor.ImplementationInstance.Should().BeNull();
            descriptor.ImplementationFactory.Should().NotBeNull();
        }

        [Test]
        public void AddTransient_Type()
        {
            var services = new ServiceCollection();
            var context = new ReplacementContext(services, typeof(ReplacementService1));

            context.AddTransient(typeof(Consumer1));

            services.Count.Should().Be(1);
            var descriptor = services.First();
            descriptor.Lifetime.Should().Be(ServiceLifetime.Transient);
            descriptor.ServiceType.Should().Be(typeof(Consumer1));
            descriptor.ImplementationType.Should().BeNull();
            descriptor.ImplementationInstance.Should().BeNull();
            descriptor.ImplementationFactory.Should().NotBeNull();
        }

        [Test]
        public void AddTransient_Type_Type()
        {
            var services = new ServiceCollection();
            var context = new ReplacementContext(services, typeof(ReplacementService1));

            context.AddTransient(typeof(IConsumer1), typeof(Consumer1));

            services.Count.Should().Be(1);
            var descriptor = services.First();
            descriptor.Lifetime.Should().Be(ServiceLifetime.Transient);
            descriptor.ServiceType.Should().Be(typeof(IConsumer1));
            descriptor.ImplementationType.Should().BeNull();
            descriptor.ImplementationInstance.Should().BeNull();
            descriptor.ImplementationFactory.Should().NotBeNull();
        }

        [Test]
        public void AddScoped_TService()
        {
            var services = new ServiceCollection();
            var context = new ReplacementContext(services, typeof(ReplacementService1));

            context.AddScoped<Consumer1>();

            services.Count.Should().Be(1);
            var descriptor = services.First();
            descriptor.Lifetime.Should().Be(ServiceLifetime.Scoped);
            descriptor.ServiceType.Should().Be(typeof(Consumer1));
            descriptor.ImplementationType.Should().BeNull();
            descriptor.ImplementationInstance.Should().BeNull();
            descriptor.ImplementationFactory.Should().NotBeNull();
        }

        [Test]
        public void AddScoped_TService_TImplementation()
        {
            var services = new ServiceCollection();
            var context = new ReplacementContext(services, typeof(ReplacementService1));

            context.AddScoped<IConsumer1, Consumer1>();

            services.Count.Should().Be(1);
            var descriptor = services.First();
            descriptor.Lifetime.Should().Be(ServiceLifetime.Scoped);
            descriptor.ServiceType.Should().Be(typeof(IConsumer1));
            descriptor.ImplementationType.Should().BeNull();
            descriptor.ImplementationInstance.Should().BeNull();
            descriptor.ImplementationFactory.Should().NotBeNull();
        }

        [Test]
        public void AddScoped_Type()
        {
            var services = new ServiceCollection();
            var context = new ReplacementContext(services, typeof(ReplacementService1));

            context.AddScoped(typeof(Consumer1));

            services.Count.Should().Be(1);
            var descriptor = services.First();
            descriptor.Lifetime.Should().Be(ServiceLifetime.Scoped);
            descriptor.ServiceType.Should().Be(typeof(Consumer1));
            descriptor.ImplementationType.Should().BeNull();
            descriptor.ImplementationInstance.Should().BeNull();
            descriptor.ImplementationFactory.Should().NotBeNull();
        }

        [Test]
        public void AddScoped_Type_Type()
        {
            var services = new ServiceCollection();
            var context = new ReplacementContext(services, typeof(ReplacementService1));

            context.AddScoped(typeof(IConsumer1), typeof(Consumer1));

            services.Count.Should().Be(1);
            var descriptor = services.First();
            descriptor.Lifetime.Should().Be(ServiceLifetime.Scoped);
            descriptor.ServiceType.Should().Be(typeof(IConsumer1));
            descriptor.ImplementationType.Should().BeNull();
            descriptor.ImplementationInstance.Should().BeNull();
            descriptor.ImplementationFactory.Should().NotBeNull();
        }

        [Test]
        public void AddSingleton_TService()
        {
            var services = new ServiceCollection();
            var context = new ReplacementContext(services, typeof(ReplacementService1));

            context.AddSingleton<Consumer1>();

            services.Count.Should().Be(1);
            var descriptor = services.First();
            descriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
            descriptor.ServiceType.Should().Be(typeof(Consumer1));
            descriptor.ImplementationType.Should().BeNull();
            descriptor.ImplementationInstance.Should().BeNull();
            descriptor.ImplementationFactory.Should().NotBeNull();
        }

        [Test]
        public void AddSingleton_TService_TImplementation()
        {
            var services = new ServiceCollection();
            var context = new ReplacementContext(services, typeof(ReplacementService1));

            context.AddSingleton<IConsumer1, Consumer1>();

            services.Count.Should().Be(1);
            var descriptor = services.First();
            descriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
            descriptor.ServiceType.Should().Be(typeof(IConsumer1));
            descriptor.ImplementationType.Should().BeNull();
            descriptor.ImplementationInstance.Should().BeNull();
            descriptor.ImplementationFactory.Should().NotBeNull();
        }

        [Test]
        public void AddSingleton_Type()
        {
            var services = new ServiceCollection();
            var context = new ReplacementContext(services, typeof(ReplacementService1));

            context.AddSingleton(typeof(Consumer1));

            services.Count.Should().Be(1);
            var descriptor = services.First();
            descriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
            descriptor.ServiceType.Should().Be(typeof(Consumer1));
            descriptor.ImplementationType.Should().BeNull();
            descriptor.ImplementationInstance.Should().BeNull();
            descriptor.ImplementationFactory.Should().NotBeNull();
        }

        [Test]
        public void AddSingleton_Type_Type()
        {
            var services = new ServiceCollection();
            var context = new ReplacementContext(services, typeof(ReplacementService1));

            context.AddSingleton(typeof(IConsumer1), typeof(Consumer1));

            services.Count.Should().Be(1);
            var descriptor = services.First();
            descriptor.Lifetime.Should().Be(ServiceLifetime.Singleton);
            descriptor.ServiceType.Should().Be(typeof(IConsumer1));
            descriptor.ImplementationType.Should().BeNull();
            descriptor.ImplementationInstance.Should().BeNull();
            descriptor.ImplementationFactory.Should().NotBeNull();
        }
    }
}
