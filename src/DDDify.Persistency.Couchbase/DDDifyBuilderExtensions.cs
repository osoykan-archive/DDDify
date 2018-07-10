using System;
using System.Collections.Concurrent;

using Couchbase.Extensions.DependencyInjection;

using DDDify.Persistency;
using DDDify.Persistency.Couchbase;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DDDify
{
    public static class DDDifyBuilderExtensions
    {
        internal static readonly ConcurrentDictionary<Type, string> BucketNames = new ConcurrentDictionary<Type, string>();

        public static IDDDifyBuilder UseCouchbase(this IDDDifyBuilder builder, IConfigurationSection configurationSection)
        {
            builder.Services.AddCouchbase(configurationSection);

            builder.Services.AddSingleton<IBucketNameResolver, BucketNameResolver>();
            builder.Services.AddTransient(typeof(ICouchbaseRepository<>), typeof(CouchbaseRepository<>));
            builder.Services.AddTransient(typeof(ICouchbaseRepository<,>), typeof(CouchbaseRepository<,>));
            builder.Services.AddTransient(typeof(IRepository<,>), typeof(CouchbaseRepository<,>));

            return builder;
        }

        public static IDDDifyBuilder UseCouchbase(this IDDDifyBuilder builder, IConfiguration configuration, string configSectionName = "Couchbase") => builder.UseCouchbase(configuration.GetSection(configSectionName));

        public static IDDDifyBuilder AddBucket<TAggregateRoot>(this IDDDifyBuilder builder, string bucketName)
        {
            BucketNames.AddOrUpdate(typeof(TAggregateRoot), bucketName, (t, v) => bucketName);
            return builder;
        }
    }
}
