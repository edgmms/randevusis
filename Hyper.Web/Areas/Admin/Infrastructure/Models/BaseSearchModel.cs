namespace Hyper.Web.Areas.Admin.Infrastructure.Models
{
    public class BaseSearchModel
    {
        public int Draw { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get;set; }
    }
}
