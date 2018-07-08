using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

using Couchbase.Core;
using Couchbase.Extensions.DependencyInjection;
using Couchbase.Linq;

using Microsoft.Extensions.DependencyInjection;

using TestBase;
using TestBase.ProductContext.Aggregates;

namespace DDDify.Persistency.Couchbase.Tests
{
    public abstract class ApplicationTestBase : TestBaseWithIoC
    {
        private readonly IDisposable dockerContainer;

        protected ApplicationTestBase()
        {
            string image = $"pcontext/couchbase_{ProduceImageSuffix}:latest";
            DockerHelper.BuildImageIfNotExists(image, "Docker\\Docker.tar").Wait();
            dockerContainer = DockerHelper.StartContainerAsync(image, ports: new List<int>
            {
                11210,
                8091,
                8092,
                8093,
                8094
            }, environment: new ReadOnlyDictionary<string, string>(new Dictionary<string, string>
            {
                { "MEMORY_QUOTA", "256" },
                { "INDEX_MEMORY_QUOTA", "256" },
                { "FTS_MEMORY_QUOTA", "256" },
                { "SERVICES", "kv,n1ql,index,fts" },
                { "USERNAME", "Administrator" },
                { "PASSWORD", "password" }
            })).Result;

            Building(services =>
            {
                services.AddDDDify(builder =>
                {
                    builder.UseCouchbase(Configuration)
                           .AddBucket<Product>("ProductContext");
                });
                services.AddLogging();
            }).Ok();
        }

        protected override IDictionary<string, string> InMemoryConfiguration
        {
            get
            {
                return new Dictionary<string, string>
                {
                    { "Couchbase:Servers:0", "http://localhost:8091" },
                    { "Couchbase:UseSsl", "false" },
                    { "Couchbase:Username", "Administrator" },
                    { "Couchbase:Password", "password" }
                };
            }
        }

        public object ProduceImageSuffix
        {
            get
            {
                return Convert.ToBase64String(Guid.NewGuid()
                                                  .ToByteArray())
                              .Replace("=", "")
                              .Replace("+", "")
                              .Replace("/", "")
                              .ToLower();
            }
        }

        protected TEntity Query<TEntity>(Expression<Func<TEntity, bool>> filter)
        {
            TEntity doc;
            do
            {
                var bucketName = The<IBucketNameResolver>().GetBucket<Product>();
                using (IBucket bucket = The<IBucketProvider>().GetBucket(bucketName))
                {
                    try
                    {
                        doc = new BucketContext(bucket).Query<TEntity>().FirstOrDefault(filter);
                    }
                    catch (Exception)
                    {
                        doc = default(TEntity);
                    }
                }
            }
            while (doc == null);

            return doc;
        }

        public override void Dispose()
        {
            dockerContainer.Dispose();
        }
    }
}
