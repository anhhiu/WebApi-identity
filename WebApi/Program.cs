using Microsoft.EntityFrameworkCore;
using WebApi.Context;
using WebApi.ServiceCollection;
using WebApi.Utilyties;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AppServiceCollection(builder.Configuration);

var connecString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(connecString, options =>
        options.EnableRetryOnFailure()));





builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// jwt

var appSettingSection = builder.Configuration.GetSection("Appsettings");
builder.Services.Configure<AppSettings>(appSettingSection);


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
