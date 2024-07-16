using FeedbackAPI.Data;
using FeedbackAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services: DbContext, Сервис для работы с БД (Scoped), AutoMapper, Controllers
builder.Services.AddDbContext<FeedbackDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MSSQL")));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddControllers();

var myCorsPolicy ="";
builder.Services.AddCors(o => o.AddPolicy(name: myCorsPolicy, builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();
app.UseCors(policyName: myCorsPolicy);

app.MapControllers();

app.Run();
