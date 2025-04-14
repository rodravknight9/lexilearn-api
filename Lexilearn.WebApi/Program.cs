using Leaxilearn.LibreTranslate;
using Lexilearn.Application;
using Lexilearn.MySql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureLibreTranslateService(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration);

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

app.UseAuthorization();

app.MapControllers();

app.Run();
