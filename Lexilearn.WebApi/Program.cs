using Lexilearn.AnkiImport;
using Lexilearn.CustomTranslate;
using Lexilearn.LibreTranslate;
using Lexilearn.Application;
using Lexilearn.Identity;
using Lexilearn.MySql;
using Lexilearn.WebApi.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
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

    options.AddSchemaTransformer((schema, context, cancellationToken) =>
    {
        RepairDuplicateSchemaReferences(schema);
        return Task.CompletedTask;
    });
});
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureLibreTranslateService();
builder.Services.AddInfrastructureCustomTranslateService();
builder.Services.AddInfrastructureAnkiImportService();
builder.Services.AddPersistenceServices(builder.Configuration, builder.Environment);
builder.Services.ConfigureIdentityService(builder.Configuration, builder.Environment);
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 100_000_000;
});
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
            .AllowAnyHeader()
            .AllowCredentials();
    }
    else if (app.Environment.IsDevelopment())
    {
        policy.SetIsOriginAllowed(_ => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
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

// System.Text.Json emits a relative $ref when the same type is used twice (headers and body).
// OpenAPI turns that into "#/components/schemas/#/...", which Swagger cannot resolve.
static void RepairDuplicateSchemaReferences(OpenApiSchema schema)
{
    if (schema.Reference?.Id is { } id && id.Contains('#'))
        schema.Reference.Id = "RequestEntry";

    if (schema.Items is not null)
        RepairDuplicateSchemaReferences(schema.Items);

    if (schema.Properties is not null)
    {
        foreach (var property in schema.Properties.Values)
            RepairDuplicateSchemaReferences(property);
    }

    foreach (var composed in new[] { schema.AllOf, schema.AnyOf, schema.OneOf })
    {
        if (composed is null)
            continue;

        foreach (var child in composed)
            RepairDuplicateSchemaReferences(child);
    }

    if (schema.AdditionalProperties is not null)
        RepairDuplicateSchemaReferences(schema.AdditionalProperties);
}

public partial class Program;
