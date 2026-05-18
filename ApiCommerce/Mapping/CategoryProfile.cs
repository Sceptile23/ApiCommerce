using ApiCommerce.Model;
using ApiCommerce.Model.Dtos;
using AutoMapper;

namespace ApiCommerce.Mapping;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryDtos>().ReverseMap();
        CreateMap<Category, CreateCategoryDtos>().ReverseMap();
    }
}