using Hyper.Core;
using Hyper.Web.Areas.Admin.Infrastructure.Models;
using System.Linq;

namespace Hyper.Web.Areas.Admin.Models.Extensions
{
    public static class DataTableExtensions
    {
        public static DataTableResponse ToDataTableResponse<TModel>(this IPagedList<TModel> data, BaseSearchModel model)
        {
            var response = new DataTableResponse
            {
                data = data,
                draw = model.Draw,
                recordsFiltered = data.TotalCount,
                recordsTotal = data.TotalCount
            };

            return response;
        }
    }
}
