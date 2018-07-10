using System.Threading;
using System.Threading.Tasks;

using DDDify.Aggregates;

namespace DDDify.Persistency
{
    public interface IRepository<TAggregateRoot, TKey>
        where TAggregateRoot : AggregateRoot<TKey>
    {
        Task<TAggregateRoot> Insert(TAggregateRoot entity, CancellationToken cancellationToken = default);

        Task<TAggregateRoot> Update(TAggregateRoot entity, CancellationToken cancellationToken = default);

        Task Delete(TAggregateRoot entity, CancellationToken cancellationToken = default);

        string GenerateDocumentId(TAggregateRoot entity);
    }
}
