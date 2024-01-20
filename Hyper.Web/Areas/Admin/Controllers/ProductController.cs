using Hyper.Core.Domain.Products;
using Hyper.Core.Results;
using Hyper.Services.Products;
using Hyper.Web.Areas.Admin.Factories.Products;
using Hyper.Web.Areas.Admin.Models.Extensions;
using Hyper.Web.Areas.Admin.Models.Products;
using Hyper.Web.Infrastructure.Mapper;
using Microsoft.AspNetCore.Mvc;

namespace Hyper.Web.Areas.Admin.Controllers
{
    public class ProductController : BaseAdminController
    {
        private readonly IProductService _productService;
        private readonly IProductFactory _productFactory;

        public ProductController(IProductService productService,
            IProductFactory productFactory)
        {
            _productService = productService;
            _productFactory = productFactory;
        }

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            var model = new ProductSearchModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual IActionResult ProductList(ProductSearchModel model)
        {
            var data = _productService.SearchProducts(
                name: model.SearchName,
                pageIndex: model.PageIndex,
                pageSize: model.PageSize
                );

            var tableResponse = data.ToDataTableResponse(model);
            return Json(tableResponse);
        }

        public virtual IActionResult Create()
        {
            var model = _productFactory.PrepareProductModel(new ProductModel());
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual IActionResult Create(ProductModel model, bool saveContinue = false)
        {
            if (!ModelState.IsValid)
                return View(model);

            var entity = model.ToEntity<Product>();
            _productService.Insert(entity);

            if (saveContinue)
                return RedirectToAction("Edit", new { id = entity.Id });

            return RedirectToAction("List");
        }

        public virtual IActionResult Edit(int id)
        {
            var entity = _productService.GetById(id);

            //already null or try to access another store's entity
            if (entity is null)
                return RedirectToAction("List");

            var model = _productFactory.PrepareProductModel(entity);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual IActionResult Edit(ProductModel model, bool saveContinue = false)
        {
            if (!ModelState.IsValid)
                return View(model);

            var entity = _productService.GetById(model.Id);

            //already null or try to access another store's entity
            if (entity is null)
                return RedirectToAction("List");

            entity = model.ToEntity(entity);
            _productService.Update(entity);

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

            _productService.DeleteById(id);
            return Json(new SuccessResult("entity deleted successfully"));
        }
    }
}
