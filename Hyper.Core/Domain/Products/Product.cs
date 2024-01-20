using System;

namespace Hyper.Core.Domain.Products
{
    public partial class Product : FullAuditedEntity<int>, IStoreEntity, ISoftDeletedEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }
        public decimal ProductCost { get; set; }
        public int TaxRate { get; set; } 
        public int ProductTypeId { get; set; }
        public ProductType ProductType
        {
            get { return (ProductType)ProductTypeId; }
            set { ProductTypeId = (int)value; }
        }

        public int RegisteredInStoreId { get; set; }
    }
}