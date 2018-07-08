using System;
using System.Collections.Generic;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TestBase
{
    public abstract class TestBaseWithIoC : IDisposable
    {
        protected IServiceProvider LocalResolver;

        protected TestBaseWithIoC() => Services = new ServiceCollection();

        protected IServiceCollection Services { get; }

        protected IConfiguration Configuration => new ConfigurationBuilder().AddInMemoryCollection(InMemoryConfiguration).Build();

        protected abstract IDictionary<string, string> InMemoryConfiguration { get; }

        public virtual void Dispose()
        {
        }

        protected TestBaseWithIoC Building(Action<IServiceCollection> builder)
        {
            builder(Services);
            return this;
        }

        public void Ok()
        {
            PreBuild();
            LocalResolver = Services.BuildServiceProvider();
            PostBuild();
        }

        protected virtual void PreBuild()
        {
        }

        protected virtual void PostBuild()
        {
        }

        protected T The<T>() => LocalResolver.GetRequiredService<T>();
        
    }
}
