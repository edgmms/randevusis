namespace Hyper.Web.Areas.Admin.Models.Appointments
{
    public class UsedProductModel 
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ProductTypeId { get; set; }
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }
        public decimal ProductCost { get; set; }
        public int TaxRate { get; set; }
    }
}
