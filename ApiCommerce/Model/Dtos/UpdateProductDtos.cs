using System;

namespace ApiCommerce.Model.Dtos;

public class UpdateProductDtos
{
    public string Name {get; set;} = string.Empty;
    public string Description {get; set;} = string.Empty;
    public decimal Price {get; set;}
    public int Stock {get; set;}
    public string imagUrl {get; set;} = string.Empty;
    public string SKU {get; set;} = string.Empty;    
    public DateTime? UpdateDate {get; set;} = null;
    public int CategoryId {get; set;}
}