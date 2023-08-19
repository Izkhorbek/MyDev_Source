using Microsoft.EntityFrameworkCore;
using SignupLogin.API.Data;
using SignupLogin.API.RegisterLogin.service;
using SignupLogin.API.RegisterLogin.service.Implementation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// DB Injection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("MysqlConnection"), ServerVersion.AutoDetect(
        builder.Configuration.GetConnectionString("MysqlConnection"))));

// Interface and Imp injection
builder.Services.AddScoped<IRegisterLogin, RegisterLogin>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Run();
