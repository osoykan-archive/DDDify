using System.Threading;
using System.Threading.Tasks;
using DDDify.Messaging.Handlers;
using TestBase.ProductContext.Aggregates;

namespace DDDify.Persistency.Couchbase.Tests.Commands
{
    public class CreateProductCommandHandler : CommandHandler<CreateProductCommand>
    {
        private readonly ICouchbaseRepository<Product> _repository;

        public CreateProductCommandHandler(ICouchbaseRepository<Product> repository) => _repository = repository;

        protected override async Task Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var product = Product.Create(command.ProductId, command.Name);

            await _repository.Insert(product, cancellationToken);
        }
    }
}