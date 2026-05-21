using System;
using ApiCommerce.Model;

namespace ApiCommerce.Repository.IRepository;

public interface IProductRepository
{
    public ICollection<Product> GetProducts();
    public ICollection<Product> GetProductForCategory(int categoryId);
    public ICollection<Product> SearchProduct(string name);
    public Product? GetProduct(int id);
    public bool BuyProduct(string name, int amount);
    public bool ProductExists(int id);
    public bool ProductExists(string name);
    public bool CreateProduct(Product product);
    public bool UpdateProduct(Product product);
    public bool DeleteProduct (Product product);
    public bool Save();
}