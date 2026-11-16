using CarBrand.Api.ExceptionHandler;
using CarBrand.Application;
using CarBrand.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";

builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<ICarBrandService, CarBrandService>();
builder.Services.AddScoped<ICarBrandRepository, CarBrandRepository>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
var app = builder.Build();


if (app.Environment.IsDevelopment() ||
    app.Environment.IsEnvironment("Testing"))
{
    using var scope = app.Services.CreateScope();

    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    dbContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();