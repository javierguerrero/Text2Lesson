using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ms.rabbitmq.Producers;
using ms.students.application.Mappers;
using ms.students.application.Queries;
using ms.students.domain.Interfaces;
using ms.students.infrastructure.Data;
using ms.students.infrastructure.Repositories;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configurar dependencias
builder.Services.AddScoped(typeof(IProducer), typeof(EventProducer));

//Habilitar el ingreso del Bearer token a través de la interfaz de Swagger
builder.Services.AddSwaggerGen(swagger =>
{
    swagger.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Students Api",
        Version = "v1"
    });
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Cabecera de autorización JWT. \r\n Introduzca ['Bearer'] [espacio] [Token].",
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                          new OpenApiSecurityScheme {
                              Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme
                                                                , Id = "Bearer" }
                          },new string[] {}
                    }
                });
});


// For Entity Framework
var connectionString = $"Server={configuration.GetConnectionString("StudentDB:HostName")};" +
                                $"Database={configuration.GetConnectionString("StudentDB:Catalogue")};" +
                                $"User ID={configuration.GetConnectionString("StudentDB:User")};" +
                                $"Password={configuration.GetConnectionString("StudentDB:Password")};" +
                                $"Encrypt=False;MultipleActiveResultSets=True;";
builder.Services.AddDbContext<StudentContext>(options => options.UseSqlServer(connectionString));

// Configurar dependencias
builder.Services.AddScoped(typeof(IStudentRepository), typeof(StudentRepository));


// Automapper
var automapperConfig = new MapperConfiguration(mapperConfig =>
{
    mapperConfig.AddMaps(typeof(StudentsMapperProfile).Assembly);
});
IMapper mapper = automapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

// MediatR
builder.Services.AddMediatR(typeof(GetAllStudentsQuery).GetTypeInfo().Assembly);


//configurar la autorización del token JWT
var privateKey = configuration.GetValue<string>("Authentication:JWT:Key");
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(privateKey)),
        ValidateLifetime = true,
        RequireExpirationTime = true,
        ClockSkew = TimeSpan.Zero
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();