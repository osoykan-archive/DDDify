using System;
using DDDify.Messaging;

namespace TestBase.ProductContext.Aggregates.Events
{
    public class ProductVariantBarcodeChanged : Event
    {
        public ProductVariantBarcodeChanged(Guid productId, Guid variantId, string barcode)
        {
            ProductId = productId;
            VariantId = variantId;
            Barcode = barcode;
        }

        public Guid ProductId { get; }

        public Guid VariantId { get; }

        public string Barcode { get; }
    }
}