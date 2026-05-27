using ApiCommerce.Model;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
public class ApplicationDBContext : DbContext
{
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
    {
        
    }

    public DbSet<Category> Categories {get; set;}
    public DbSet<Product> Products {get; set;}
    public DbSet<User> Users {get; set;}
}