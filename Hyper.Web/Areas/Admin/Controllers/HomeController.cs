using Microsoft.AspNetCore.Mvc;

namespace Hyper.Web.Areas.Admin.Controllers
{
    public class HomeController : BaseAdminController
    {
        public IActionResult Index()
        {
            return Redirect("/Admin/Appointment");
        }
    }
}
