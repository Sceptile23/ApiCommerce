using System;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace ApiCommerce.Model;

public class Category
{
    //PARAMETROS DE ESTA ENTIDAD
    [Key]
    public int Id {get; set;}
    [Required(ErrorMessage = "Se necesita que agregue el nombre.")]
    public string Name {get; set;} = string.Empty;
    [Required]
    public DateTime CreationDate {get; set;}
}