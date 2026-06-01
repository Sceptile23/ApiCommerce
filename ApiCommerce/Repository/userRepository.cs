using System;
using Apicommerce.Model.Dtos;
using ApiCommerce.Model;
using ApiCommerce.Model.Dtos;
using ApiCommerce.Repository.IRepository;
using BCrypt;
using BCrypt.Net;

namespace ApiCommerce.Repository;

public class userRepository : IUserRepository
{
    public readonly ApplicationDBContext _db;

    public userRepository(ApplicationDBContext db)
    {
        this._db = db;
    }

    public ICollection<User> GetUsers()
    {
        return _db.Users.OrderBy(u => u.Name).ToList();
    } 

    public User? GetUser (int id)
    {
        return _db.Users.FirstOrDefault(u => u.Id == id);
    }

    public bool IsUniqueUser(string name)
    {
        return !_db.Users.Any(u => u.UserName.ToLower().Trim() == name.ToLower().Trim());
    }

    public Task<UserLoginResponseDto> Login (UserLoginDto userLogin)
    {
        throw new NotImplementedException();
    }

    public Task<User> Register(CreateUserDto createUserDto)
    {
        var encriptedPassword = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);
        var user = new User
        {
            Name = createUserDto.Name,
            UserName = createUserDto.UserName ?? "No user name.",
            Password = encriptedPassword,
            Role = createUserDto.Role
        };
        throw new NotImplementedException();
    }
}