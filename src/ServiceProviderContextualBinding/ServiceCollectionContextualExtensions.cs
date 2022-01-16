using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceProviderContextualBinding
{
    public static class ServiceCollectionContextualExtensions
    {
        /// <summary>
        /// Specifies the implementation type to be used in place of the default implementation for the specified service type.
        /// </summary>
        /// <typeparam name="TService">The type of the service for which the default implementation is to be replaced.</typeparam>
        /// <typeparam name="TReplacement">
        /// The implementation type of the service to be used instead of the default implementation.
        /// </typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> instance to use.</param>
        /// <returns>
        /// A new instance of <see cref="ReplacementContext"/> that can be used to add registrations to the
        /// <paramref name="services"/> collection.
        /// </returns>
        /// <remarks>
        /// While the use of <typeparamref name="TService"/> has no direct influence on the resolution of services, it is 
        /// recommended to use this overload to explicitly indicate the intent to replace the implementation of the specified
        /// service, and ensure that the replacement implements that service.
        /// </remarks>
        public static ReplacementContext WithReplacement<TService, TReplacement>(this IServiceCollection services)
            where TService : class
            where TReplacement : class, TService 
            => new ReplacementContext(services, typeof(TReplacement));

        /// <summary>
        /// Specifies the implementation type to be used in place of the default implementation for the specified service type.
        /// </summary>
        /// <typeparam name="TService">The type of the service for which the default implementation is to be replaced.</typeparam>
        /// <typeparam name="TReplacement">
        /// The implementation type of the service to be used instead of the default implementation.
        /// </typeparam>
        /// <param name="context">
        /// A source replacement context to be copied and extended with <typeparamref name="TReplacement"/>.
        /// </param>
        /// <returns>
        /// A new instance of <see cref="ReplacementContext"/> that includes all of the replacements from the source context in
        /// addition to <typeparamref name="TReplacement"/>.
        /// </returns>
        /// <remarks>
        /// While the use of <typeparamref name="TService"/> has no direct influence on the resolution of services, it is 
        /// recommended to use this overload to explicitly indicate the intent to replace the implementation of the specified
        /// service, and ensure that the replacement implements that service.
        /// </remarks>
        public static ReplacementContext WithReplacement<TService, TReplacement>(this ReplacementContext context)
            where TService : class
            where TReplacement : class, TService 
            => new ReplacementContext(context, typeof(TReplacement));

        /// <summary>
        /// Specifies the implementation type to be used in place of the default implementation for the service type implemented
        /// by the implementation type.
        /// </summary>
        /// <typeparam name="TReplacement">
        /// The implementation type of the service to be used instead of the default implementation.
        /// </typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> instance to use.</param>
        /// <returns>
        /// A new instance of <see cref="ReplacementContext"/> that can be used to add registrations to the
        /// <paramref name="services"/> collection.
        /// </returns>
        public static ReplacementContext WithReplacement<TReplacement>(this IServiceCollection services)
            where TReplacement : class
            => new ReplacementContext(services, typeof(TReplacement));

        /// <summary>
        /// Specifies the implementation type to be used in place of the default implementation for the service type implemented
        /// by the implementation type.
        /// </summary>
        /// <typeparam name="TReplacement">
        /// The implementation type of the service to be used instead of the default implementation.
        /// </typeparam>
        /// <param name="context">
        /// A source replacement context to be copied and extended with <typeparamref name="TReplacement"/>.
        /// </param>
        /// <returns>
        /// A new instance of <see cref="ReplacementContext"/> that includes all of the replacements from the source context in
        /// addition to <typeparamref name="TReplacement"/>.
        /// </returns>
        public static ReplacementContext WithReplacement<TReplacement>(this ReplacementContext context)
            where TReplacement : class
            => new ReplacementContext(context, typeof(TReplacement));

        /// <summary>
        /// Specifies the implementation type to be used in place of the default implementation for the service type implemented
        /// by the implementation type.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> instance to use.</param>
        /// <param name="replacementType">
        /// The implementation type of the service to be used instead of the default implementation.
        /// </param>
        /// <returns>
        /// A new instance of <see cref="ReplacementContext"/> that can be used to add registrations to the
        /// <paramref name="services"/> collection.
        /// </returns>
        public static ReplacementContext WithReplacement(
            this IServiceCollection services,
            Type replacementType)
            => new ReplacementContext(services, replacementType);

        /// <summary>
        /// Specifies the implementation type to be used in place of the default implementation for the service type implemented
        /// by the implementation type.
        /// </summary>
        /// <param name="context">
        /// A source replacement context to be copied and extended with <paramref name="replacementType"/>.
        /// </param>
        /// <param name="replacementType">
        /// The implementation type of the service to be used instead of the default implementation.
        /// </param>
        /// <returns>
        /// A new instance of <see cref="ReplacementContext"/> that includes all of the replacements from the source context in
        /// addition to <typeparamref name="TReplacement"/>.
        /// </returns>
        public static ReplacementContext WithReplacement(
            this ReplacementContext context,
            Type replacementType)
            => new ReplacementContext(context, replacementType);

        /// <summary>
        /// Adds a transient service of the type specified in <typeparamref name="TService"/> to the 
        /// <see cref="IServiceCollection"/> instance in the context, using the replacement types in the context.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="context">
        /// An instance of <see cref="ReplacementContext"/> containing the <see cref="IServiceCollection"/> instance and 
        /// replacement types to use.
        /// </param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static ReplacementContext AddTransient<TService>(this ReplacementContext context)
            where TService : class
            => context.AddTransient(typeof(TService));

        /// <summary>
        /// Adds a transient service of the type specified in <typeparamref name="TService"/> to the 
        /// <see cref="IServiceCollection"/> instance in the context, using the replacement types in the context.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="context">
        /// An instance of <see cref="ReplacementContext"/> containing the <see cref="IServiceCollection"/> instance and 
        /// replacement types to use.
        /// </param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static ReplacementContext AddTransient<TService, TImplementation>(this ReplacementContext context)
            where TService : class
            where TImplementation : class, TService
            => context.AddTransient(typeof(TService), typeof(TImplementation));

        /// <summary>
        /// Adds a transient service of the type specified in <paramref name="serviceType"/> to the 
        /// <see cref="IServiceCollection"/> instance in the context, using the replacement types in the context.
        /// </summary>
        /// <param name="context">
        /// An instance of <see cref="ReplacementContext"/> containing the <see cref="IServiceCollection"/> instance and 
        /// replacement types to use.
        /// </param>
        /// <param name="serviceType">The type of the service to add.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static ReplacementContext AddTransient(
            this ReplacementContext context,
            Type serviceType)
            => context.AddTransient(serviceType, serviceType);

        /// <summary>
        /// Adds a transient service of the type specified in <paramref name="serviceType"/> to the 
        /// <see cref="IServiceCollection"/> instance in the context, using the replacement types in the context.
        /// </summary>
        /// <param name="context">
        /// An instance of <see cref="ReplacementContext"/> containing the <see cref="IServiceCollection"/> instance and 
        /// replacement types to use.
        /// </param>
        /// <param name="serviceType">The type of the service to add.</param>
        /// <param name="implementationType">The type of the implementation to use.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static ReplacementContext AddTransient(
            this ReplacementContext context,
            Type serviceType,
            Type implementationType)
            => context.Add(serviceType, implementationType, ServiceLifetime.Transient);

        /// <summary>
        /// Adds a scoped service of the type specified in <typeparamref name="TService"/> to the 
        /// <see cref="IServiceCollection"/> instance in the context, using the replacement types in the context.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="context">
        /// An instance of <see cref="ReplacementContext"/> containing the <see cref="IServiceCollection"/> instance and 
        /// replacement types to use.
        /// </param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static ReplacementContext AddScoped<TService>(this ReplacementContext context)
            where TService : class
            => context.AddScoped(typeof(TService));

        /// <summary>
        /// Adds a scoped service of the type specified in <typeparamref name="TService"/> to the 
        /// <see cref="IServiceCollection"/> instance in the context, using the replacement types in the context.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="context">
        /// An instance of <see cref="ReplacementContext"/> containing the <see cref="IServiceCollection"/> instance and 
        /// replacement types to use.
        /// </param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static ReplacementContext AddScoped<TService, TImplementation>(this ReplacementContext context)
            where TService : class
            where TImplementation : class, TService
            => context.AddScoped(typeof(TService), typeof(TImplementation));

        /// <summary>
        /// Adds a scoped service of the type specified in <paramref name="serviceType"/> to the 
        /// <see cref="IServiceCollection"/> instance in the context, using the replacement types in the context.
        /// </summary>
        /// <param name="context">
        /// An instance of <see cref="ReplacementContext"/> containing the <see cref="IServiceCollection"/> instance and 
        /// replacement types to use.
        /// </param>
        /// <param name="serviceType">The type of the service to add.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static ReplacementContext AddScoped(
            this ReplacementContext context,
            Type serviceType)
            => context.AddScoped(serviceType, serviceType);

        /// <summary>
        /// Adds a scoped service of the type specified in <paramref name="serviceType"/> to the 
        /// <see cref="IServiceCollection"/> instance in the context, using the replacement types in the context.
        /// </summary>
        /// <param name="context">
        /// An instance of <see cref="ReplacementContext"/> containing the <see cref="IServiceCollection"/> instance and 
        /// replacement types to use.
        /// </param>
        /// <param name="serviceType">The type of the service to add.</param>
        /// <param name="implementationType">The type of the implementation to use.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static ReplacementContext AddScoped(
            this ReplacementContext context,
            Type serviceType,
            Type implementationType)
            => context.Add(serviceType, implementationType, ServiceLifetime.Scoped);

        /// <summary>
        /// Adds a singleton service of the type specified in <typeparamref name="TService"/> to the 
        /// <see cref="IServiceCollection"/> instance in the context, using the replacement types in the context.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="context">
        /// An instance of <see cref="ReplacementContext"/> containing the <see cref="IServiceCollection"/> instance and 
        /// replacement types to use.
        /// </param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static ReplacementContext AddSingleton<TService>(this ReplacementContext context)
            where TService : class
            => context.AddSingleton(typeof(TService));

        /// <summary>
        /// Adds a singleton service of the type specified in <typeparamref name="TService"/> to the 
        /// <see cref="IServiceCollection"/> instance in the context, using the replacement types in the context.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="context">
        /// An instance of <see cref="ReplacementContext"/> containing the <see cref="IServiceCollection"/> instance and 
        /// replacement types to use.
        /// </param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static ReplacementContext AddSingleton<TService, TImplementation>(this ReplacementContext context)
            where TService : class
            where TImplementation : class, TService
            => context.AddSingleton(typeof(TService), typeof(TImplementation));

        /// <summary>
        /// Adds a singleton service of the type specified in <paramref name="serviceType"/> to the 
        /// <see cref="IServiceCollection"/> instance in the context, using the replacement types in the context.
        /// </summary>
        /// <param name="context">
        /// An instance of <see cref="ReplacementContext"/> containing the <see cref="IServiceCollection"/> instance and 
        /// replacement types to use.
        /// </param>
        /// <param name="serviceType">The type of the service to add.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static ReplacementContext AddSingleton(
            this ReplacementContext context,
            Type serviceType)
            => context.AddSingleton(serviceType, serviceType);

        /// <summary>
        /// Adds a singleton service of the type specified in <paramref name="serviceType"/> to the 
        /// <see cref="IServiceCollection"/> instance in the context, using the replacement types in the context.
        /// </summary>
        /// <param name="context">
        /// An instance of <see cref="ReplacementContext"/> containing the <see cref="IServiceCollection"/> instance and 
        /// replacement types to use.
        /// </param>
        /// <param name="serviceType">The type of the service to add.</param>
        /// <param name="implementationType">The type of the implementation to use.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static ReplacementContext AddSingleton(
            this ReplacementContext context,
            Type serviceType,
            Type implementationType)
            => context.Add(serviceType, implementationType, ServiceLifetime.Singleton);

        /// <summary>
        /// Adds a service of the type specified in <paramref name="serviceType"/> to the <see cref="IServiceCollection"/> 
        /// instance in the context, using the replacement types in the context.
        /// </summary>
        /// <param name="context">
        /// An instance of <see cref="ReplacementContext"/> containing the <see cref="IServiceCollection"/> instance and 
        /// replacement types to use.
        /// </param>
        /// <param name="serviceType">The type of the service to add.</param>
        /// <param name="implementationType">The type of the implementation to use.</param>
        /// <param name="lifetime">The <see cref="ServiceLifetime"/> of the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
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
