using Hyper.Web.Areas.Admin.Models.Reports;

namespace Hyper.Web.Areas.Admin.Factories.Reports
{
	public interface IReportFactory
	{
		CashReportModel PrepareCacheReportModel();
	}
}
