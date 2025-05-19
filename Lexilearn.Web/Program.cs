using Lexilearn.Web;
using Lexilearn.Web.Services.Implementation;
using Lexilearn.Web.Services.Interfaces;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<ITranslationService, TranslationService>();


await builder.Build().RunAsync();

//dotnet run --urls "http://0.0.0.0:7139"