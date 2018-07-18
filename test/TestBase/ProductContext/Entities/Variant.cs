using System;
using DDDify.Entities;

namespace TestBase.ProductContext.Entities
{
    public class Variant : Entity<Guid>
    {
        private Variant()
        {
        }

        public Variant(long productId,
            string barcode) : this()
        {
            Id = Guid.NewGuid();
            ProductId = productId;
            Barcode = barcode;
        }

        public long ProductId { get; }

        public string Barcode { get; }
    }
}