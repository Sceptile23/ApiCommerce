using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Apicommerce.Model.Dtos;
using ApiCommerce.Model;
using ApiCommerce.Model.Dtos;
using ApiCommerce.Repository.IRepository;
using BCrypt;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ApiCommerce.Repository;

public class userRepository : IUserRepository
{
    public readonly ApplicationDBContext _db;
    public string? SecretKey;

    public userRepository(ApplicationDBContext db, IConfiguration configuration)
    {
        this._db = db;
        this.SecretKey = configuration.GetValue<string>("ApiSettings:SecretKey");
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

    public async Task<UserLoginResponseDto> Login (UserLoginDto userLogin)
    {
        if (string.IsNullOrEmpty(userLogin.UserName) || string.IsNullOrEmpty(userLogin.Password))
        {
            return new UserLoginResponseDto
            {
                user = null,
                Token = " ",
                Message = "Username and password ar required."
            };
        }

        var user = await _db.Users.FirstOrDefaultAsync(u => u.UserName.ToLower().Trim() == userLogin.UserName.ToLower().Trim());

        if (user == null)
        {
            return new UserLoginResponseDto
            {
                user = null,
                Token = " ",
                Message = "User not found."
            };
        }

        if (!BCrypt.Net.BCrypt.Verify(userLogin.Password, user.Password))
        {
            return new UserLoginResponseDto
            {
                user = null,
                Token = " ",
                Message = "Incorrect password."
            };
        }
        //JWT
        var handlerToken = new JwtSecurityTokenHandler();
        if (string.IsNullOrEmpty(SecretKey))
        {
            throw new InvalidOperationException("Secretkey is not configurated.");
        }
        var key = Encoding.UTF8.GetBytes(SecretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new []
            {
                new Claim("id", user.Id.ToString()),
                new Claim("UserName", user.UserName),
                new Claim(ClaimTypes.Role, user.Role ?? string.Empty)
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = handlerToken.CreateToken(tokenDescriptor);
        return new UserLoginResponseDto
        {
            user = new UserRegisterDto
            {
                Name = user.Name,
                UserName = user.UserName,
                Role = user.Role,
                Password = user.Password
            },
            Token = handlerToken.WriteToken(token),
            Message = "Login successfull."
        };
    }

    public async Task<User> Register(CreateUserDto createUserDto)
    {
        var encriptedPassword = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);
        var user = new User
        {
            Name = createUserDto.Name,
            UserName = createUserDto.UserName ?? "No user name.",
            Password = encriptedPassword,
            Role = createUserDto.Role
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }
}