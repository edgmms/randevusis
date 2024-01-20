using FluentValidation;

namespace Hyper.Web.Infrastructure.Validation
{
    public abstract partial class BaseValidation<TModel> : AbstractValidator<TModel> where TModel : class
    {

    }
}
