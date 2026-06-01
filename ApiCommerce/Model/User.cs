using System;
using System.ComponentModel.DataAnnotations;

namespace ApiCommerce.Model;

public class User
{
    //PROPIEDADES DEL USUARIO.
    [Key]
    public int Id {get; set;}
    public string? Name {get; set;}
    [Required]
    public string UserName {get; set;} = string.Empty;
    [Required]
    public string? Password {get; set;}
    [Required]
    public string? Role {get; set;}
}