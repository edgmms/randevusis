using FluentValidation;
using Hyper.Web.Areas.Admin.Models.Products;
using Hyper.Web.Infrastructure.Validation;

namespace Hyper.Web.Areas.Admin.Validations.Products
{
    public class ProductValidator : BaseValidation<ProductModel>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Name)
             .NotEmpty()
             .WithMessage("Ürün / Hizmet adı gereklidir.");

            RuleFor(x => x.Price)
             .GreaterThanOrEqualTo(0)
             .WithMessage("Fiyat 0 veya 0 dan büyük olmalıdır.");

            RuleFor(x => x.OldPrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Eski fiyat 0 veya 0 dan büyük olmalıdır.");

            RuleFor(x => x.ProductCost)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Maliyet 0 veya 0 dan büyük olmalıdır.");
        }
    }
}
