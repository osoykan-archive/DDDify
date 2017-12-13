using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using DDDify.Aggregates;

namespace DDDify.Repositories
{
    /// <summary>
    ///     This interface is implemented by all repositories to ensure implementation of fixed methods.
    /// </summary>
    /// <typeparam name="TAggregateRoot">Main Entity type this repository works on</typeparam>
    /// <typeparam name="TPrimaryKey">Primary key type of the entity</typeparam>
    public interface IRepository<TAggregateRoot, TPrimaryKey> : IRepository where TAggregateRoot : class, IAggregateRoot<TPrimaryKey>
    {
        /// <summary>
        ///     Used to get a IQueryable that is used to retrieve entities from entire table.
        /// </summary>
        /// <returns>IQueryable to be used to select entities from database</returns>
        IQueryable<TAggregateRoot> GetAll();

        /// <summary>
        ///     Gets an entity with given primary key.
        /// </summary>
        /// <param name="id">Primary key of the entity to get</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Entity</returns>
        Task<TAggregateRoot> Get(TPrimaryKey id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        ///     Gets an entity with given given predicate or null if not found.
        /// </summary>
        /// <param name="predicate">Predicate to filter entities</param>
        /// <param name="cancellationToken"></param>
        Task<TAggregateRoot> Get(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        ///     Inserts a new entity.
        /// </summary>
        /// <param name="entity">Inserted entity</param>
        /// <param name="cancellationToken"></param>
        Task Insert(TAggregateRoot entity, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        ///     Updates an existing entity.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <param name="cancellationToken"></param>
        Task Update(TAggregateRoot entity, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        ///     Updates an existing entity.
        /// </summary>
        /// <param name="id">Id of the entity</param>
        /// <param name="updateAction">Action that can be used to change values of the entity</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Updated entity</returns>
        Task Update(TPrimaryKey id, Func<TAggregateRoot, Task> updateAction, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        ///     Deletes an entity.
        /// </summary>
        /// <param name="entity">Entity to be deleted</param>
        /// <param name="cancellationToken"></param>
        Task Delete(TAggregateRoot entity, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        ///     Deletes an entity by primary key.
        /// </summary>
        /// <param name="id">Primary key of the entity</param>
        /// <param name="cancellationToken"></param>
        Task Delete(TPrimaryKey id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        ///     Deletes many entities by function.
        ///     Notice that: All entities fits to given predicate are retrieved and deleted.
        ///     This may cause major performance problems if there are too many entities with
        ///     given predicate.
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        /// <param name="cancellationToken"></param>
        Task Delete(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));
    }
}
