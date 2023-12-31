using ApiPeliculas.Data;
using ApiPeliculas.Models;
using ApiPeliculas.PeliculasMapper;
using ApiPeliculas.Repository;
using ApiPeliculas.Repository.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//configuramos conexion a sql server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSql"));
});

//soporte para .net identity
builder.Services.AddIdentity<AppUsuario, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

//Añadir cache
builder.Services.AddResponseCaching();

//add mapper
builder.Services.AddAutoMapper(typeof(PeliculasMapper));

//Se configura autenticación
var key = builder.Configuration.GetValue<string>("ApiSettings:Secreta");
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

//add dependency injection
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IPeliculaRepository, PeliculaRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

// Add services to the container.

builder.Services.AddControllers(option =>
{
    //cache profile 
    option.CacheProfiles.Add("PorDefecto20Segundos", new CacheProfile() { Duration = 20 });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//add documentation for JWT
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description= "Autenticación JWT usando el esquema Bearer. \r\n\r" + 
        "Ingresa la palabra Beare seguida de un espacio y después su token en el campo de abajo \r\n\r" +
        "Ejemplo: \"Bearer dafasdfasdfas\"" ,
        Name="Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference()
                {
                    Type=ReferenceType.SecurityScheme,
                    Id ="Bearer"
                },
                Scheme ="oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

//SOPORTE PARA CORS
/*
 *SE PUEDE HABILITAR: 
 *1. UN DOMINIO
 *2. MULTIPLES DOMINOS
 *3. CUALQUIER DOMINIO (TENER EN CUENTA SEGURIDAD)
 *Usamos de ejemplo el dominio: https://localhost:3222, se debe cambiar por el correcto
 *SE usa (*) para todos los dominios
 */
builder.Services.AddCors(p => p.AddPolicy("PolicyCors", build =>
{
    //build.WithOrigins("https://localhost:7291/").AllowAnyMethod().AllowAnyHeader();
    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//soporte para cors
app.UseCors("PolicyCors");
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
