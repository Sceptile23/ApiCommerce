using System;

namespace ApiCommerce.Model.Dtos;

public class UserRegisterDto
{
    public int? id {get; set;}

    public required string? UserName {get; set;}
    public required string? Password {get; set;}
    public string? Name {get; set;}
    public string? Role {get; set;}
}