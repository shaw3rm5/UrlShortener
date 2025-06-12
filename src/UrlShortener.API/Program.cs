using Microsoft.AspNetCore.Diagnostics;
using UrlShortener.API.Middlewares;
using UrlShortener.Application;
using UrlShortener.Infrastructure;
using UrlShortener.Infrastructure.CassandraBuilder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<CassandraConfiguration>(
    builder.Configuration.GetSection("CassandraConfiguration"));

builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddCors();

builder.Services.AddInfrastructureDependencies();
builder.Services.AddApplicationDependencies();
var app = builder.Build();

app.UseCors();
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
