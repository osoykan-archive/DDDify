using System;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace DDDify
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDDDify(this IServiceCollection services)
        {
            services.AddMediatR();
            return services;
        }

        public static IServiceCollection AddDDDify(this IServiceCollection services, Action<IDDDifyBuilder> builderAction)
        {
            if (builderAction == null)
            {
                throw new ArgumentNullException(nameof(builderAction));
            }

            services.AddDDDify();
            var builder = new DDDifyBuilder(services);
            builderAction(builder);

            return services;
        }
    }
}
