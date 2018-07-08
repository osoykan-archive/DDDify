using System;

using DDDify.Messaging;

namespace TestBase.ProductContext.Aggregates.Events
{
    public class VariantAddedToProduct : Event
    {
        public VariantAddedToProduct(Guid productId, string barcode)
        {
            ProductId = productId;
            Barcode = barcode;
        }

        public Guid ProductId { get; }

        public string Barcode { get; }
    }
}
