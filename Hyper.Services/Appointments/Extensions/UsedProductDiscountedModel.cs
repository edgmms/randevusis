namespace Hyper.Services.Appointments.Extensions
{
    public class UsedProductDiscountedModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal DiscountedPrice { get; set; }
        public int TaxRate { get; set; }
    }
}
