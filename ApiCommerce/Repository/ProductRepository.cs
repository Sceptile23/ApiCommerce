using System;
using ApiCommerce.Repository.IRepository;
using ApiCommerce.Model;
using System.Linq.Expressions;
using ApiCommerce.Model.Dtos;
using Microsoft.Identity.Client;


namespace ApiCommerce.Repository;

public class ProductRepository : IProductRepository
{
    //DECLARAMOS EL CONTEXTO EN SOLO LECTURA 
    public readonly ApplicationDBContext _db;

    //CONTRUCTOR PARA PODER INYECTAR EL CONTEXTO EN EL REPOSITORIO
    public ProductRepository (ApplicationDBContext db)
    {
        this._db = db;
    }

    //AHORA PODEMOS CREAR LOS METODOS PARA MANEJAR LOS PRODUCTOS
    public ICollection<Product> GetProducts()
    {
        var products = _db.Products.OrderBy(p => p.Name).ToList();
        return products;
    }

    public ICollection<Product> GetProductForCategory(int categoryId)
    {
        if(categoryId <= 0)
        {
            return new List<Product>();
        }
        var products = _db.Products.Where(p => p.CategoryId == categoryId).ToList();
        return products;
    }

    public ICollection<Product> SearchProduct(string name)
    {
        
        IQueryable<Product> query = _db.Products;
        if (!string.IsNullOrEmpty(name))
        {
            query = query.Where(p => p.Name.ToLower().Trim() == name.ToLower().Trim());
        }
        return query.ToList();
    }

    public Product? GetProduct(int id)
    {
        var product = _db.Products.FirstOrDefault(p => p.ProductId == id);
        return product;
    }

    public bool BuyProduct(string name, int amount)
    {
        if(string.IsNullOrWhiteSpace(name) || amount <= 0)
        {
            return false;
        }

        var product = _db.Products.FirstOrDefault(p => p.Name == name);
        if (product == null)
        {
            throw new ArgumentException("Product not found");
        }

        if (product.Stock < amount)
        {
            throw new ArgumentException("no enough stock");
        }

        product.Stock -= amount;
        return UpdateProduct(product);
    }

    public bool ProductExists(int id)
    {
        if(id <= 0)
        {
            return false;
        }
        return _db.Products.Any(p => p.ProductId == id);
    }

    public bool ProductExists(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }
        return _db.Products.Any(p => p.Name.ToLower().Trim() == name.ToLower().Trim());
    }

    public bool CreateProduct(Product product)
    {
        if(product == null)
        {
            return false;
        }
        product.CreationDate = DateTime.Now;
        product.UpdateDate = DateTime.Now;
        _db.Products.Add(product);
        return Save();
    }

    public bool UpdateProduct(Product product)
    {
        if(product == null)
        {
            return false;
        }
        product.UpdateDate = DateTime.Now;
        _db.Products.Update(product);
        return Save();
    }

    public bool DeleteProduct(Product product)
    {
        _db.Products.Remove(product);
        return Save();
    }

    public bool Save()
    {
        bool changes = _db.SaveChanges() >= 0 ? true : false;
        return changes;
    }
}