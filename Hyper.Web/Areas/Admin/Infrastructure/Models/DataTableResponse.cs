using System.Collections.Generic;

namespace Hyper.Web.Areas.Admin.Infrastructure.Models
{
    public class DataTableResponse
    {
        public int draw { get; set; }
        public int recordsFiltered { get; set; }
        public int recordsTotal { get; set; }
        public object data { get; set; }
    }
}
