using System;
using System.Linq;

using DDDify.Aggregates;

namespace DDDify.Persistency.Couchbase
{
    public interface ICouchbaseRepository<TAggregateRoot> : ICouchbaseRepository<TAggregateRoot, Guid>
        where TAggregateRoot : AggregateRoot<Guid>
    {
    }

    public interface ICouchbaseRepository<TAggregateRoot, TKey> : IRepository<TAggregateRoot, TKey>
        where TAggregateRoot : AggregateRoot<TKey>
    {
        IQueryable<TAggregateRoot> Get();
    }
}
