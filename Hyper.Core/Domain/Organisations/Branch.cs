namespace Hyper.Core.Domain.Organisations
{
    public class Branch : FullAuditedEntity<int>, IStoreEntity
    {
        public int RegisteredInStoreId { get; set; }
        public string Name { get; set; }
        public string TaxAdministration { get; set; }
        public string TaxNumber { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int CompanyId { get; set; }
  
    }
}
