using Hyper.Data;
using Hyper.Services.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Hyper.Web.Controllers
{
    public class HomeController : BasePublicController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}