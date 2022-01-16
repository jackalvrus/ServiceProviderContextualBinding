# ServiceProviderContextualBinding - An extension for [Microsoft.Extensions.DependencyInjection](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection?view=dotnet-plat-ext-6.0) to create contextual bindings.

The default Microsoft dependency injection (DI) provider does not provide many of the advanced features of third party providers like [Autofac](https://autofac.org/) and [Ninject](http://www.ninject.org/). One of the features that it does not provide is contextual binding. This package provides a simple form of contextual binding where the default implementation for one or more services can be replaced on a per-registration basis.

## Example

In this example, the default implementation of the `IService` interface is the `DefaultService` class. When registering the `Consumer` class with the DI container, we specify that we want to resolve the `IService` interface using the `ReplacementService` class instead.

``` CSharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IService, DefaultService>();
        services.AddSingleton<ReplacementService>();
        services.WithReplacement<IService, ReplacementService>()
            .AddSingleton<Consumer>();
    }
}

public interface IService {}

public class DefaultService : IService {}

public class ReplacementService : IService {}

public class Consumer
{
    public Consumer(IService service)
    {
        // the service parameter is an instance of ReplacementService
    }
}
```

## Registering Replacement Services

Services must be registered with the container in order to be used as replacements. If a replacement service is not registered, an exception will be thrown when the service that references the replacement is resolved.

You should not register replacements as the implemented service type. For example, avoid this:

``` CSharp
services.AddSingleton<IService, DefaultService>();
services.AddSingleton<IService, ReplacementService>(); // avoid this
```

The simplest way to avoid this is to register the replacement class directly with the DI container as shown in the original example.

If you prefer not to register classes, only interfaces, you can use a marker interface that inherits from the service interface. Then specify that marker interface as the replacement. For example:

``` CSharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IServiceAlternate, ReplacementService>();
        services.WithReplacement<IService, IServiceAlternate>()
            .AddSingleton<Consumer>();
    }
}

public interface IService {}

// IServiceAlternate is the marker interface
public interface IServiceAlternate : IService {}

public class ReplacementService : IServiceAlternate {}
```

## Implementing Replacement Services

Classes to be used as replacements must implement the service type to be replaced. You should avoid implementing multiple service types in the same class as it can cause unexpected results when the same consumer uses more than one of the implemented interfaces. For example, avoid this:

``` CSharp
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<ReplacementService>();
        services.WithReplacement<ReplacementService>()
            .AddSingleton<Consumer>();
    }
}

public interface IService1 {}

public interface IService2 {}

public class ReplacementService : IService1, IService2 {}

public class Consumer
{
    public Consumer(IService1 service1, IService2 service2)
    {
        // only one of the arguments will refer to the replacement service
    }
}
```

## Using Replacements

Each call to `WithReplacement` creates a new instance of a "replacement context". Calls to `WithReplacement` can be chained to create a context containing multiple replacements. For example:

``` CSharp
services.WithReplacement<IService1, ReplacementService1>()
    .WithReplacement<IService2, ReplacementService2>()
    .AddSingleton<Consumer>(); // both IService1 and IService2 are replaced
```

Multiple services can be registered using the same replacement context. For example:

``` CSharp
services.WithReplacement<IService, ReplacementService>()
    .AddSingleton<Consumer1>()  // receives ReplacementService
    .AddSingleton<Consumer2>(); // also receives ReplacementService
```

Since each call to `WithReplacement` creates a new context, calls to `WithReplacement` can be interleaved with calls to `Add*`, like so:

``` CSharp
services.WithReplacement<IService1, ReplacementService1>()
    .AddSingleton<Consumer1>() // receives ReplacementService1
    .WithReplacement<IService2, ReplacementService2>()
    .AddSingleton<Consumer2>(); // receives ReplacementService1 and ReplacementService2
```

## Service Registration Overloads

As much as possible, all `Add*` overloads (`AddSingleton`, `AddTransient`, and `AddScoped`) that are provided for `IServiceCollection` are provided by this package. The one major exception is: none of the overloads that take a factory delegate are provided.

Internally, this package uses [`ActivatorUtilities.CreateInstance`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.activatorutilities.createinstance?view=dotnet-plat-ext-6.0) to create service instances. If you need to use a factory delegate for your service registration, you can use `ActivatorUtilities.CreateInstance` directly instead of this package.
