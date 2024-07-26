using FeedbackAPI.Data;
using FeedbackAPI.Data.Entities;
using FeedbackAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using FluentValidation;
using FeedbackAPI.Validators;
using FeedbackAPI.Dto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FeedbackAPI.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services: DbContext, Сервис для работы с БД (Scoped)
builder.Services.AddDbContext<FeedbackDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MSSQL")));
//builder.Services.AddDbContext<FeedbackAuthDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MSSQL")));
// Подключаем unit of work с репозиториями
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//Подключаем валидаторы
builder.Services.AddScoped<IValidator<MessageTopicDto>, MessageTopicValidator>();
builder.Services.AddScoped <IValidator<MessageDto>, MessageValidator>();
builder.Services.AddScoped <IValidator<ContactDto>, ContactValidator>();
builder.Services.AddScoped<IValidator<MessagePostDto>, MessagePostValidator>();
//Добавляем HTTP Client для отправки POST капча
builder.Services.AddHttpClient();
//Подключаем автомаппер
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddControllers(); 
//Подключаем парсер JSON
builder.Services.AddControllers().AddNewtonsoftJson();
//Добавляем аунтефикацию
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = false,
        ValidIssuer = builder.Configuration["Jwt:Issuer"]!,
        ValidAudience = builder.Configuration["Jwt:Audience"]!,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!))

    };

});

builder.Services.AddAuthorization(options => options.DefaultPolicy =
    new AuthorizationPolicyBuilder
    (JwtBearerDefaults.AuthenticationScheme)
    .RequireAuthenticatedUser()
    .Build());

builder.Services.AddIdentity<User, IdentityRole<long>>()
    .AddEntityFrameworkStores < FeedbackDbContext>()
    .AddUserManager<UserManager<User>>()
    .AddSignInManager<SignInManager<User>>();

builder.Services.AddScoped<ITokenService, TokenService>();

//Установка политики CORS
var myCorsPolicy ="";
builder.Services.AddCors(o => o.AddPolicy(name: myCorsPolicy, builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));

builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "formapi", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.

app.UseAuthorization();
app.UseAuthentication();
app.UseCors(policyName: myCorsPolicy);

app.MapControllers();

app.Run();
