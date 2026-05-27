using System;
using ApiCommerce.Model.Dtos;

namespace Apicommerce.Model.Dtos;

public class UserLoginResponseDto
{
    public UserRegisterDto? user {get; set;}
    public string? Token {get; set;}
    public string? Message {get; set;}
}