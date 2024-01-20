using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hyper.Web.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class BaseAdminController : Controller
    {
     
    }
}
