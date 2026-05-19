using ApiCommerce.Model;
using ApiCommerce.Model.Dtos;
using AutoMapper;

namespace ApiCommerce.Mapping.CategoryProfile;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryDtos>().ReverseMap();
        CreateMap<Category, CreateCategoryDtos>().ReverseMap();
    }
}