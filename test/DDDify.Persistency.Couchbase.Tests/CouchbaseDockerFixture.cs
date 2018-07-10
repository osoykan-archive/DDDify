using System;

using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Extensions;
using Ductus.FluentDocker.Services;

using Xunit;

namespace DDDify.Persistency.Couchbase.Tests
{
    public class CouchbaseDockerFixture : IDisposable
    {
        private readonly ICompositeService _image;
        private readonly IContainerService _runningImage;

        public CouchbaseDockerFixture()
        {
            string resourcePath = $"{GetType().Assembly.GetName().Name}/{GetType().Assembly.GetName().Name}.Docker/configure.sh";
            string resourcePath2 = $"{GetType().Assembly.GetName().Name}/{GetType().Assembly.GetName().Name}.Docker/by_productId.ddoc";

            _image = new Builder().DefineImage("pcontext/couchbase_test")
                                  .From("couchbase:latest")
                                  .ExposePorts(8091, 8092, 8093, 8094, 11210)
                                  .Add($"emb:{resourcePath}", "/opt/couchbase/configure.sh")
                                  .Add($"emb:{resourcePath2}", "/opt/couchbase/by_productId.ddoc")
                                  .Command("/opt/couchbase/configure.sh")
                                  .Builder()
                                  .Build()
                                  .Start();

            _runningImage = new Builder().UseContainer()
                                         .UseImage("pcontext/couchbase_test")
                                         .ExposePort(8091, 8091)
                                         .ExposePort(8092, 8092)
                                         .ExposePort(8093, 8093)
                                         .ExposePort(8094, 8094)
                                         .ExposePort(11210, 11210)
                                         .Build().Start();

            _runningImage.WaitForRunning();
        }

        public void Dispose()
        {
            _image?.Dispose();
            _runningImage?.Dispose();
        }
    }

    [CollectionDefinition("Couchbase")]
    public class CouchbaseCollection : ICollectionFixture<CouchbaseDockerFixture>
    {
    }
}
