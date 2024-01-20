using Hyper.Core.Domain.Patients;
using Hyper.Core.Results;
using Hyper.Services.Patients;
using Hyper.Web.Areas.Admin.Models.Extensions;
using Hyper.Web.Areas.Admin.Models.Patients;
using Hyper.Web.Infrastructure.Mapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace Hyper.Web.Areas.Admin.Controllers
{
    public class PatientController : BaseAdminController
    {
        private readonly IPatientService _patientService;
        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            var model = new PatientSearchModel();
            return View(model);
        }

        [HttpGet]
        public virtual IActionResult GetPatientsByName(string patientName)
        {
            var data = _patientService.SearchPatients(name: patientName);
            var result = new SuccessDataResult(data.Select(x => new { text = x.FullName, id = x.Id.ToString() }).ToList());
            return Json(result);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual IActionResult PatientList(PatientSearchModel model)
        {
            var data = _patientService.SearchPatients(
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
            var model = new PatientModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual IActionResult Create(PatientModel model, bool saveContinue = false)
        {
            if (!ModelState.IsValid)
                return View(model);

            var entity = model.ToEntity<Patient>();
            _patientService.Insert(entity);

            if (saveContinue)
                return RedirectToAction("Edit", new { id = entity.Id });

            return RedirectToAction("List");
        }


        public virtual IActionResult CreateAjax()
        {
            var model = new PatientModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual IActionResult CreateAjax(PatientModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ErrorResult());

            var entity = model.ToEntity<Patient>();
            _patientService.Insert(entity);

            return Ok(new SuccessResult());
        }


        public virtual IActionResult Edit(int id)
        {
            var entity = _patientService.GetById(id);
            var model = entity.ToModel<PatientModel>();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual IActionResult Edit(PatientModel model, bool saveContinue = false)
        {
            if (!ModelState.IsValid)
                return View(model);

            var entity = _patientService.GetById(model.Id);
            entity = model.ToEntity(entity);
            _patientService.Update(entity);

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

            _patientService.DeleteById(id);
            return Json(new SuccessResult("entity deleted successfully"));
        }
    }
}
