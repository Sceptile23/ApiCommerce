using ApiCommerce.Repository;
using ApiCommerce.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var DBConnectionString = builder.Configuration.GetConnectionString("ConexionSql");
builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(DBConnectionString));

//APARTADO PARA CATEGORIA
builder.Services.AddScoped<ICategoryRepository, categoryRepository>();

//APARTADO PARA PRODUCTOS
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserRepository, userRepository>();

//PARA QUE SE PUEDE HACER EL AUTOMAPEO
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//CONFIGURACION CORS 
builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowFrontend",
        
        policy =>
        {
            policy.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
        });
    }
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//USO DE LA CONFIGURACION BASICA DE CORS 
app.UseCors("AllowFrontend");

app.Run();
