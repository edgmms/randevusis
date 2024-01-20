using Hyper.Core;
using Hyper.Web.Areas.Admin.Factories.Reports;
using Microsoft.AspNetCore.Mvc;

namespace Hyper.Web.Areas.Admin.Controllers
{
    public class ReportController : BaseAdminController
    {
        private readonly IReportFactory _reportFactory;
        private readonly IWorkContext _workContext;

        public ReportController(IReportFactory reportFactory, IWorkContext workContext)
        {
            _reportFactory = reportFactory;
            _workContext = workContext;
        }

        public IActionResult CashReport()
        {
            var cashReportModel = _reportFactory.PrepareCacheReportModel();
            return View(cashReportModel);
        }
    }
}
