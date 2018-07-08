using System;
using System.Linq;
using System.Reflection;

using MediatR;

namespace Microsoft.Extensions.DependencyInjection
{
    public interface IDDDifyBuilder
    {
        IServiceCollection Services { get; }

        IDDDifyBuilder WithAssemblies(params Assembly[] assemblies);
    }

    public class DDDifyBuilder : IDDDifyBuilder
    {
        public IServiceCollection Services { get; }

        public DDDifyBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IDDDifyBuilder WithAssemblies(params Assembly[] assemblies)
        {
            Assembly[] _assemblies =  AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic).ToArray();
            Services.AddMediatR(assemblies ?? _assemblies);

            return this;
        }
    }
}
