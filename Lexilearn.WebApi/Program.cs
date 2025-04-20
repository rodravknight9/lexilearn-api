using Leaxilearn.LibreTranslate;
using Lexilearn.Application;
using Lexilearn.Identity;
using Lexilearn.MySql;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureLibreTranslateService(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.ConfigureIdentityService(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddControllers();
/*builder.WebHost.ConfigureKestrel((opt =>
{
    opt.ListenAnyIP(5000);
}));*/

var app = builder.Build();

app.UseCors(x => 
    x.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(opt => {
        opt.SwaggerEndpoint("/openapi/v1.json", "API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
