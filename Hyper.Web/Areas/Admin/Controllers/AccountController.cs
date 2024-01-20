using Hyper.Core;
using Hyper.Core.Extensions;
using Hyper.Services.ApplicationUsers;
using Hyper.Web.Areas.Admin.Models.ApplicationUsers;
using Microsoft.AspNetCore.Mvc;

namespace Hyper.Web.Areas.Admin.Controllers
{
    public class AccountController : BaseAdminController
    {
        private readonly IApplicationUserService _applicationUserService;
        private readonly IWorkContext _workContext;

        public AccountController(IApplicationUserService applicationUserService,
            IWorkContext workContext)
        {
            _applicationUserService = applicationUserService;
            _workContext = workContext;
        }

        public IActionResult Profile()
        {
            var applicationUser = _workContext.CurrentApplicationUser;
            var model = new ProfileModel()
            {
                FullName = applicationUser.FullName,
                FirstName = applicationUser.FirstName,
                LastName = applicationUser.LastName,
                DateOfBirth = applicationUser.DateOfBirth.ToTurkishDateTimeNull(),
                Email = applicationUser.Email,
                Fax = applicationUser.Fax,
                Gender = applicationUser.Gender,
                Phone = applicationUser.Phone,
                StreetAddress = applicationUser.StreetAddress,
                ZipPostalCode = applicationUser.ZipPostalCode
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Profile(ProfileModel model)
        {
            var applicationUser = _workContext.CurrentApplicationUser;
            applicationUser.DateOfBirth = model.DateOfBirth.ToTurkishDateTimeNull();
            applicationUser.Email = model.Email;
            applicationUser.Fax = model.Fax;
            applicationUser.Gender = model.Gender;
            applicationUser.FirstName = model.FirstName;
            applicationUser.LastName = model.LastName;
            applicationUser.Phone = model.Phone;
            applicationUser.StreetAddress = model.StreetAddress;
            applicationUser.ZipPostalCode = model.ZipPostalCode;
            _applicationUserService.Update(applicationUser);

            return RedirectToAction("Profile");
        }


        public IActionResult ChangePassword()
        {
            var model = new ChangePasswordModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("ChangePassword");



            return RedirectToAction("ChangePassword");
        }
    }
}
