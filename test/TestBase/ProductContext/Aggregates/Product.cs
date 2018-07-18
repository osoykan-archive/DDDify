using System;
using DDDify.Aggregates;
using TestBase.ProductContext.Aggregates.Events;

namespace TestBase.ProductContext.Aggregates
{
    public partial class Product : AggregateRoot<Guid>
    {
        protected Product()
        {
            Register<ProductNameChanged>(When);
            Register<ProductCreated>(When);
        }

        public string Name { get; protected set; }

        public void ChangeName(string withName)
        {
            ApplyChange(
                new ProductNameChanged(Id, withName)
            );
        }

        public static Product Create(Guid productId, string name)
        {
            var product = new Product();
            product.ApplyChange(
                new ProductCreated(productId, name)
            );

            return product;
        }

        public void AddVariant(string barcode)
        {
            ApplyChange(
                new VariantAddedToProduct(Id, barcode)
            );
        }

        public void ChangeVariantBarcode(Guid variantId, string barcode)
        {
            ApplyChange(
                new ProductVariantBarcodeChanged(Id, variantId, barcode)
            );
        }
    }
}