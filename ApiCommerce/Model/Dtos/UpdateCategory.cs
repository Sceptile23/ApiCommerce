using System;
using System.ComponentModel.DataAnnotations;

namespace ApiCommerce.Model.Dtos;

public class UpdateCategory
{
    [Required]
    [MaxLength (50, ErrorMessage = "Required a max 50 characters")]
    [MinLength(10, ErrorMessage = "Required a min 10 characters")]
    public string Name {get; set;} = string.Empty;
}