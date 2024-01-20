using Hyper.Core;
using Hyper.Core.Domain.ApplicationUsers;
using Hyper.Services.Authentication;
using Hyper.Web.Areas.Admin.Models.ApplicationUsers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Hyper.Web.Areas.Admin.ViewComponents.ApplicationUsers
{
    public class UserProfileViewComponent : ViewComponent
    {
        private readonly IWorkContext _workContext;

        public UserProfileViewComponent(IWorkContext workContext)
        {
            _workContext = workContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new ProfileModel
            {
            };

            await Task.Run(() =>
            {
                model.FullName = _workContext.CurrentApplicationUser.FullName;
            });

            return View(model);
        }

    }
}
