using DesafioFullStack.Application.Mappings;
using DesafioFullStack.Domain.Interfaces;
using DesafioFullStack.Domain.Services;
using DesafioFullStack.Infrastructure.Data;
using DesafioFullStack.Infrastructure.Repositories;
using DesafioFullStack.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar Repositórios
builder.Services.AddScoped<IEmpresaRepository, EmpresaRepository>();
builder.Services.AddScoped<IFornecedorRepository, FornecedorRepository>();

// Registrar Domain Services
builder.Services.AddScoped<IFornecedorDomainService, FornecedorDomainService>();

// Configurar AutoMapper
builder.Services.AddAutoMapper(typeof(DesafioFullStack.Application.Mappings.MappingProfile).Assembly);

// Registrar HttpClient para CepService
builder.Services.AddHttpClient<ICepService, CepService>();

// Configurar CORS para o frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

AppDomain.CurrentDomain.FirstChanceException += (sender, eventArgs) =>
{
    if (eventArgs.Exception is ReflectionTypeLoadException rtlEx)
    {
        foreach (var e in rtlEx.LoaderExceptions)
        {
            Console.WriteLine("LOADER EXCEPTION: " + e.Message);
        }
    }
};

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();