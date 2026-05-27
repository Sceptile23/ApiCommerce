using System;
using Apicommerce.Model.Dtos;
using ApiCommerce.Model;
using ApiCommerce.Model.Dtos;

namespace ApiCommerce.Repository.IRepository;

public interface IUserRepository
{
    //CREAMOS LOS METODOS QUE TENDRÁN EL REPOSITORIO
    ICollection<User> GetUsers();
    User? GetUser (int id);
    bool IsUniqueUser (string name);
    Task<UserLoginResponseDto> Login (UserLoginDto userLogin);
    Task<User> Register (CreateUserDto user);
}