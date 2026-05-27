using System;
using System.ComponentModel.DataAnnotations;

namespace ApiCommerce.Model.Dtos;

public class CreateUserDto
{
    [Required (ErrorMessage = "The name is required.")]
    public string? Name {get; set;}
    [Required (ErrorMessage = "The UserName is required.")]
    public string? UserName {get; set;}
    [Required (ErrorMessage = "The Password is required.")]
    public string? Password {get; set;}
    [Required (ErrorMessage = "The Role is required.")]
    public string? Role {get; set;}
}