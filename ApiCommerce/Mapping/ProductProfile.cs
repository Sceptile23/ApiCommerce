using System;
using ApiCommerce.Model;
using ApiCommerce.Model.Dtos;
using AutoMapper;

namespace ApiCommerce.Mapping;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, CreateProductDtos>().ReverseMap();
        CreateMap<Product, UpdateProductDtos>().ReverseMap();
        CreateMap<Product, ProductDtos>()
        .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.category.Name))
        .ReverseMap();
    }
}