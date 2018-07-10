using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Couchbase.Core;
using Couchbase.Extensions.DependencyInjection;
using Couchbase.Linq;

using TestBase;
using TestBase.ProductContext.Aggregates;

namespace DDDify.Persistency.Couchbase.Tests
{
    public abstract class ApplicationTestBase : TestBaseWithIoC
    {
        protected override IDictionary<string, string> InMemoryConfiguration => new Dictionary<string, string>
        {
            { "Couchbase:Servers:0", "http://localhost:8091" },
            { "Couchbase:UseSsl", "false" },
            { "Couchbase:Username", "Administrator" },
            { "Couchbase:Password", "password" }
        };

        protected TEntity Query<TEntity>(Expression<Func<TEntity, bool>> filter)
        {
            TEntity doc;
            do
            {
                var bucketName = The<IBucketNameResolver>().GetBucket<Product>();
                IBucket bucket = The<IBucketProvider>().GetBucket(bucketName);
                try
                {
                    doc = new BucketContext(bucket).Query<TEntity>().FirstOrDefault(filter);
                }
                catch (Exception)
                {
                    doc = default(TEntity);
                }
            }
            while (doc == null);

            return doc;
        }

    }
}
