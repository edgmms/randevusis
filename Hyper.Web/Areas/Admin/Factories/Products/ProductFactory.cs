using Hyper.Core.Domain.Products;
using Hyper.Web.Areas.Admin.Models.Products;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Hyper.Web.Areas.Admin.Factories.Products
{
    public class ProductFactory : IProductFactory
    {
        public ProductModel PrepareProductModel(Product entity, ProductModel model = null)
        {
            if (model == null)
                model = new ProductModel();

            model.Id = entity.Id;
            model.Name = entity.Name;
            model.Description = entity.Description;
            model.Price = entity.Price;
            model.OldPrice = entity.OldPrice;
            model.ProductCost = entity.ProductCost;
            model.TaxRate= entity.TaxRate;

            model.ProductTypeId = entity.ProductTypeId;
            model.AvailableProductTypes = new List<SelectListItem>
            {
                new SelectListItem { Text = "Ürün", Value = "10", Selected = entity.ProductType == ProductType.Product },
                new SelectListItem { Text = "Hizmet", Value = "20", Selected = entity.ProductType == ProductType.Service },
            };

            return model;
        }


        public ProductModel PrepareProductModel(ProductModel model, Product entity = null)
        {
            if (entity != null)
            {
                model.Id = entity.Id;
                model.Name = entity.Name;
                model.Description = entity.Description;
                model.Price = entity.Price;
                model.OldPrice = entity.OldPrice;
                model.ProductCost = entity.ProductCost;
                model.ProductTypeId = entity.ProductTypeId;
                model.TaxRate = entity.TaxRate;
            }

            model.AvailableProductTypes = new List<SelectListItem>
            {
                new SelectListItem { Text = "Ürün", Value = "10", Selected = entity != null && entity.ProductType == ProductType.Product },
                new SelectListItem { Text = "Hizmet", Value = "20", Selected = entity != null && entity.ProductType == ProductType.Product },
            };

            return model;
        }
    }
}
