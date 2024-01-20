using Hyper.Core.Domain.Employees;
using Hyper.Core.Results;
using Hyper.Data;
using Hyper.Services.Employees;
using Hyper.Web.Areas.Admin.Models.Employees;
using Hyper.Web.Areas.Admin.Models.Extensions;
using Hyper.Web.Infrastructure.Mapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hyper.Web.Areas.Admin.Controllers
{
    public class EmployeeController : BaseAdminController
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            var model = new EmployeeSearchModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual IActionResult EmployeeList(EmployeeSearchModel model)
        {
            var data = _employeeService.SearchEmployees(
                name: model.SearchName,
                surname: model.SearchSurname,
                turkishIdentityNumber: model.SearchTurkishIdentityNumber,
                gender: model.Gender,
                email: model.SearchEmail,
                phone: model.SearchPhone,
                address: model.SearchAddress,
                loadOnlyEmailNotificationActive: model.SearchSendEmailNotifications,
                loadOnlySmsNotificationActive: model.SearchSendSmsNotifications,
                pageIndex: model.PageIndex,
                pageSize: model.PageSize
                );

            var tableResponse = data.ToDataTableResponse(model);
            return Json(tableResponse);
        }

        public virtual IActionResult Create()
        {
            var model = new EmployeeModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual IActionResult Create(EmployeeModel model, bool saveContinue = false)
        {
            if (!ModelState.IsValid)
                return View(model);

            var entity = model.ToEntity<Employee>();
            _employeeService.Insert(entity);

            if (saveContinue)
                return RedirectToAction("Edit", new { id = entity.Id });

            return RedirectToAction("List");
        }

        public virtual IActionResult Edit(int id)
        {
            var entity = _employeeService.GetById(id);
            var model = entity.ToModel<EmployeeModel>();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual IActionResult Edit(EmployeeModel model, bool saveContinue = false)
        {
            if (!ModelState.IsValid)
                return View(model);

            var entity = _employeeService.GetById(model.Id);
            entity = model.ToEntity(entity);
            _employeeService.Update(entity);

            if (saveContinue)
                return RedirectToAction("Edit", new { id = entity.Id });

            return RedirectToAction("List");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual IActionResult Delete(int id)
        {
            if (id <= 0)
                return Json(new ErrorResult("id cannot be less than zero"));

            _employeeService.DeleteById(id);
            return Json(new SuccessResult("entity deleted successfully"));
        }
    }
}
