using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Couchbase;
using Couchbase.Core;
using Couchbase.Extensions.DependencyInjection;

using DDDify.Aggregates;
using DDDify.Messaging;

using MediatR;

namespace DDDify.Persistency.Couchbase
{
    public class CouchbaseRepository<TAggregateRoot> : CouchbaseRepository<TAggregateRoot, Guid>, ICouchbaseRepository<TAggregateRoot>
        where TAggregateRoot : AggregateRoot<Guid>
    {
        public CouchbaseRepository(IBucketProvider bucketProvider, IBucketNameResolver bucketNameNameResolver, IMediator mediator) : base(bucketProvider, bucketNameNameResolver, mediator)
        {
        }
    }

    public class CouchbaseRepository<TAggregateRoot, TKey> : ICouchbaseRepository<TAggregateRoot, TKey>
        where TAggregateRoot : AggregateRoot<TKey>
    {
        private readonly IBucket _bucket;
        private readonly IMediator _mediator;

        public CouchbaseRepository(IBucketProvider bucketProvider, IBucketNameResolver bucketNameNameResolver, IMediator mediator)
        {
            _mediator = mediator;
            _bucket = bucketProvider.GetBucket(bucketNameNameResolver.GetBucket<TAggregateRoot>());
        }

        public async Task<TAggregateRoot> Insert(TAggregateRoot entity, CancellationToken cancellationToken = default)
        {
            IDocumentResult<TAggregateRoot> result = await _bucket.InsertAsync(new Document<TAggregateRoot>
            {
                Content = entity,
                Id = GenerateDocumentId(entity)
            });

            result.EnsureSuccess();

            await DispatchDomainEventsAsync(entity, cancellationToken);

            return result.Content;
        }

        public async Task<TAggregateRoot> Update(TAggregateRoot entity, CancellationToken cancellationToken = default)
        {
            IDocumentResult<TAggregateRoot> result = await _bucket.UpsertAsync(new Document<TAggregateRoot>
            {
                Content = entity,
                Id = GenerateDocumentId(entity)
            });

            result.EnsureSuccess();

            await DispatchDomainEventsAsync(entity, cancellationToken);

            return result.Content;
        }

        public async Task Delete(TAggregateRoot entity, CancellationToken cancellationToken = default)
        {
            IOperationResult result = await _bucket.RemoveAsync(new Document<TAggregateRoot>
            {
                Content = entity,
                Id = GenerateDocumentId(entity)
            });
            
            result.EnsureSuccess();

            await DispatchDomainEventsAsync(entity, cancellationToken);
        }

        public string GenerateDocumentId(TAggregateRoot entity) => $"{typeof(TAggregateRoot).Name}:{entity.Id}";

        private async Task DispatchDomainEventsAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken)
        {
            List<Event> domainEvents = aggregateRoot.GetChanges().OfType<Event>().ToList();
            aggregateRoot.ClearChanges();

            IEnumerable<Task> tasks = domainEvents
                .Select(async domainEvent => await _mediator.Publish(domainEvent, cancellationToken));

            await Task.WhenAll(tasks);
        }
    }
}
