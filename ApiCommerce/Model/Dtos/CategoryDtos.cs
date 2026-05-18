using System;
using System.ComponentModel.DataAnnotations;

namespace ApiCommerce.Model.Dtos;

public class CategoryDtos
{
    //PARAMETROS DE ESTA ENTIDAD
    public int Id {get; set;}
    public string Name {get; set;} = string.Empty;
    public DateTime CreationDate {get; set;}
}