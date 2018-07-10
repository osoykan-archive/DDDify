using System;

namespace DDDify.Persistency.Couchbase
{
    public class BucketNameResolver : IBucketNameResolver
    {
        
        public string GetBucket<TAggregateRoot>()
        {
            if (DDDifyBuilderExtensions.BucketNames.TryGetValue(typeof(TAggregateRoot), out string bucketName))
            {
                return bucketName;
            }

            throw new Exception("Bucket could not be found");
        }
    }
}