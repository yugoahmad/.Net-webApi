using AutoMapper;
using Northwind.Entities.Models;
using Northwind.Entities.DataTransferObject;

namespace NorthwindWebApi.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //get category
            CreateMap<Category, CategoryDto>();

            //post category
            CreateMap<CategoryDto, Category>();

            //get customer
            CreateMap<Customer, CustomerDto>();
            
            CreateMap<CustomerDto, Customer>();
        }
    }
}
