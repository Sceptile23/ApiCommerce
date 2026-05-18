using System;
using System.ComponentModel.DataAnnotations;

namespace ApiCommerce.Model.Dtos;

public class CreateCategoryDtos
{
    [Required(ErrorMessage = "Name required")]
    [MaxLength(50, ErrorMessage = "The name cannot exced 50 characters")]
    [MinLength(3, ErrorMessage = "The name cannot be less than 3 characters")]
    public string Name {get; set;} = string.Empty;
}