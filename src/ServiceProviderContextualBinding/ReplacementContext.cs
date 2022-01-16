using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace ServiceProviderContextualBinding
{
    /// <summary>
    /// Captures the context required to register services with replacement dependencies.
    /// </summary>
    public struct ReplacementContext
    {
        public ReplacementContext(
            IServiceCollection services, 
            Type replacementType)
        {
            if (replacementType == null) throw new ArgumentNullException(nameof(replacementType));
            Services = services ?? throw new ArgumentNullException(nameof(services));
            ReplacementTypes = new[] { replacementType };
        }

        public ReplacementContext(
            ReplacementContext source,
            Type replacementType)
        {
            if (replacementType == null) throw new ArgumentNullException(nameof(replacementType));
            Services = source.Services;
            ReplacementTypes = new List<Type>(source.ReplacementTypes) { replacementType }.ToArray();
        }

        public IServiceCollection Services { get; }
        public Type[] ReplacementTypes { get; }
    }
}
