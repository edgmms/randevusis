using AutoMapper;
using Hyper.Core.Domain.Employees;
using Hyper.Core.Domain.Patients;
using Hyper.Core.Domain.Products;
using Hyper.Core.Extensions;
using Hyper.Web.Areas.Admin.Models.Employees;
using Hyper.Web.Areas.Admin.Models.Patients;
using Hyper.Web.Areas.Admin.Models.Products;
using System;

namespace Hyper.Web.Areas.Admin.Infrastructure.Mapper
{
    public class HyperProfile : Profile
    {
        public HyperProfile()
        {
            CreateMap<Employee, EmployeeModel>()
            .ForMember(model => model.DateOfBirth, opt => opt.MapFrom(x => x.DateOfBirth.ToTurkishDateTimeNull()));

            CreateMap<EmployeeModel, Employee>()
            .ForMember(model => model.DateOfBirth, opt => opt.MapFrom(x => x.DateOfBirth.ToTurkishDateTimeNull()));

            CreateMap<Patient, PatientModel>()
            .ForMember(model => model.DateOfBirth, opt => opt.MapFrom(x => x.DateOfBirth.ToTurkishDateTimeNull()));

            CreateMap<PatientModel, Patient>()
            .ForMember(model => model.DateOfBirth, opt => opt.MapFrom(x => x.DateOfBirth.ToTurkishDateTimeNull()));

            CreateMap<Product, ProductModel>();
            CreateMap<ProductModel, Product>();
        }
    }
}
