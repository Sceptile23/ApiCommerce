using System;
using ApiCommerce.Model;
using ApiCommerce.Repository.IRepository;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ApiCommerce.Repository;

public class categoryRepository : ICategoryRepository
{
    //CREAMOS EL BLOQUE QUE SE CONTECTE A LA BASE DE DATOS
    private readonly ApplicationDBContext _db;

    public categoryRepository(ApplicationDBContext db)
    {
        this._db = db;
    }

    public ICollection<Category> GetCategories()
    {
        return _db.Categories.OrderBy(c => c.Name).ToList();
    }

    public Category? GetCategory(int id)
    {
        return _db.Categories.FirstOrDefault(c => c.Id == id);
    }

    public bool CategoryExists (int id)
    {
        return _db.Categories.Any(c => c.Id == id);
    }

    public bool CategoryExists (string name)
    {
        return _db.Categories.Any(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
    }

    public bool CreateCateogry (Category category)
    {
        category.CreationDate = DateTime.Now;
        //ALMACENAR DENTRO DE LA TABLA LOS DATOS
        _db.Categories.Add(category);
        return Save();
    }

    public bool UpdateCategory (Category category)
    {
        //ACTUALIZAR DENTRO DE LA TABLA DE DATOS
        category.CreationDate = DateTime.Now;
        _db.Categories.Update(category);
        return Save();
    }

    public bool DeleteCategory (Category category)
    {
        //ELIMINAR DATOS DENTRO DE LA TABLA 
        _db.Categories.Remove(category);
        return Save();
    }

    public bool Save()
    {
        bool changes = _db.SaveChanges() >= 0 ? true : false;
        return changes;
    }
}