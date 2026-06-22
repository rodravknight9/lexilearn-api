using Lexilearn.LibreTranslate;
using Lexilearn.Application;
using Lexilearn.Identity;
using Lexilearn.MySql;
using Lexilearn.WebApi.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.Configure<CorsSettings>(builder.Configuration.GetSection("CorsSettings"));
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "Paste the JWT from POST /api/Auth/Login (token only, no 'Bearer ' prefix)."
        };
        return Task.CompletedTask;
    });

    options.AddOperationTransformer((operation, context, cancellationToken) =>
    {
        if (context.Description.ActionDescriptor.EndpointMetadata.OfType<IAuthorizeData>().Any())
        {
            operation.Security =
            [
                new OpenApiSecurityRequirement
                {
                    [new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    }] = Array.Empty<string>()
                }
            ];
        }
        return Task.CompletedTask;
    });
});
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureLibreTranslateService(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration, builder.Environment);
builder.Services.ConfigureIdentityService(builder.Configuration, builder.Environment);
builder.Services.AddAuthorization();
builder.Services.AddControllers();
/*builder.WebHost.ConfigureKestrel((opt =>
{
    opt.ListenAnyIP(5000);
}));*/

var app = builder.Build();

var corsSettings = builder.Configuration.GetSection("CorsSettings").Get<CorsSettings>() ?? new CorsSettings();
app.UseCors(policy =>
{
    if (corsSettings.AllowedOrigins.Length > 0)
    {
        policy.WithOrigins(corsSettings.AllowedOrigins)
            .AllowAnyMethod()
            .AllowAnyHeader();
    }
    else if (app.Environment.IsDevelopment())
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    }
});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(opt =>
    {
        opt.SwaggerEndpoint("/openapi/v1.json", "API v1");
        opt.RoutePrefix = "swagger";
        opt.EnablePersistAuthorization();
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program;
