using System;

namespace DDDify.Persistency.Couchbase
{
    public interface IBucketNameResolver
    {
        string GetBucket<TAggregateRoot>();
    }
}
