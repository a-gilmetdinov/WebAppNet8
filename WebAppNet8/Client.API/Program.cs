using Microsoft.EntityFrameworkCore;
using WebAppNet8.Client.Application.Services;
using WebAppNet8.Client.Application.Services.Interfaces;
using WebAppNet8.Client.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Регистрация сервисов
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection")));

builder.Services.AddScoped<IClientService, ClientService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Настройка Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();