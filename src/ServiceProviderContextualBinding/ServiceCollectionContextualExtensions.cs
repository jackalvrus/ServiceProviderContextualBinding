using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceProviderContextualBinding
{
    public static class ServiceCollectionContextualExtensions
    {
        public static ReplacementContext WithReplacement<TService, TReplacement>(this IServiceCollection services)
            where TService : class
            where TReplacement : class, TService 
            => new ReplacementContext(services, typeof(TReplacement));

        public static ReplacementContext WithReplacement<TService, TReplacement>(this ReplacementContext context)
            where TService : class
            where TReplacement : class, TService 
            => new ReplacementContext(context, typeof(TReplacement));

        public static ReplacementContext WithReplacement<TReplacement>(this IServiceCollection services)
            where TReplacement : class
            => new ReplacementContext(services, typeof(TReplacement));

        public static ReplacementContext WithReplacement<TReplacement>(this ReplacementContext context)
            where TReplacement : class
            => new ReplacementContext(context, typeof(TReplacement));

        public static ReplacementContext WithReplacement(
            this IServiceCollection services,
            Type replacementType)
            => new ReplacementContext(services, replacementType);

        public static ReplacementContext WithReplacement(
            this ReplacementContext context,
            Type replacementType)
            => new ReplacementContext(context, replacementType);

        public static ReplacementContext AddTransient<TService>(this ReplacementContext context)
            where TService : class
            => context.AddTransient(typeof(TService));

        public static ReplacementContext AddTransient<TService, TImplementation>(this ReplacementContext context)
            where TService : class
            where TImplementation : class, TService
            => context.AddTransient(typeof(TService), typeof(TImplementation));

        public static ReplacementContext AddTransient(
            this ReplacementContext context,
            Type serviceType)
            => context.AddTransient(serviceType, serviceType);

        public static ReplacementContext AddTransient(
            this ReplacementContext context,
            Type serviceType,
            Type implementationType)
            => context.Add(serviceType, implementationType, ServiceLifetime.Transient);

        public static ReplacementContext AddScoped<TService>(this ReplacementContext context)
            where TService : class
            => context.AddScoped(typeof(TService));

        public static ReplacementContext AddScoped<TService, TImplementation>(this ReplacementContext context)
            where TService : class
            where TImplementation : class, TService
            => context.AddScoped(typeof(TService), typeof(TImplementation));

        public static ReplacementContext AddScoped(
            this ReplacementContext context,
            Type serviceType)
            => context.AddScoped(serviceType, serviceType);

        public static ReplacementContext AddScoped(
            this ReplacementContext context,
            Type serviceType,
            Type implementationType)
            => context.Add(serviceType, implementationType, ServiceLifetime.Scoped);

        public static ReplacementContext AddSingleton<TService>(this ReplacementContext context)
            where TService : class
            => context.AddSingleton(typeof(TService));

        public static ReplacementContext AddSingleton<TService, TImplementation>(this ReplacementContext context)
            where TService : class
            where TImplementation : class, TService
            => context.AddSingleton(typeof(TService), typeof(TImplementation));

        public static ReplacementContext AddSingleton(
            this ReplacementContext context,
            Type serviceType)
            => context.AddSingleton(serviceType, serviceType);

        public static ReplacementContext AddSingleton(
            this ReplacementContext context,
            Type serviceType,
            Type implementationType)
            => context.Add(serviceType, implementationType, ServiceLifetime.Singleton);

        public static ReplacementContext Add(
            this ReplacementContext context, 
            Type serviceType, 
            Type implementationType, 
            ServiceLifetime lifetime)
        {
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationType == null) throw new ArgumentNullException(nameof(implementationType));

            var replacements = context.ReplacementTypes;
            var item = new ServiceDescriptor(
                serviceType, 
                serviceProvider => ActivatorUtilities.CreateInstance(
                    serviceProvider, 
                    implementationType, 
                    replacements.Select(t => serviceProvider.GetRequiredService(t)).ToArray()), 
                lifetime);
            context.Services.Add(item);

            return context;
        }
    }
}
