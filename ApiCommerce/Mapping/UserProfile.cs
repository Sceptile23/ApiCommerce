using System;
using Apicommerce.Model.Dtos;
using ApiCommerce.Model;
using ApiCommerce.Model.Dtos;
using AutoMapper;

namespace ApiCommerce.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<User, CreateUserDto>().ReverseMap();
        CreateMap<User, UserLoginDto>().ReverseMap();
        CreateMap<User, UserLoginResponseDto>().ReverseMap();
    }
}