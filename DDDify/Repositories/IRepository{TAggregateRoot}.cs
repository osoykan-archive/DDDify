using DDDify.Aggregates;
using DDDify.Entities;

namespace DDDify.Repositories
{
    public interface IRepository<TAggregateRoot> : IRepository<TAggregateRoot, int> where TAggregateRoot : class, IAggregateRoot<int>
    {
    }
}
