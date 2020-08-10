using System;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Brightcast.BaseRepository;
using Brightcast.Client.Data;
using Brightcast.Controls;
using Brightcast.Controls.Grid;
using Brightcast.Model;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Brightcast.Client
{
    public class Program
    {
        public const string BaseClient = "Brightcast.ServerAPI";
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>(nameof(App).ToLowerInvariant());

            builder.Services.AddHttpClient(BaseClient,
                client =>
                client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddTransient(sp =>
            sp.GetRequiredService<IHttpClientFactory>()
            .CreateClient(BaseClient));

            builder.Services.AddApiAuthorization();

            // client implementation
            builder.Services.AddScoped<IBasicRepository<Contact>, WasmRepository>();
            builder.Services.AddScoped<IUnitOfWork<Contact>, WasmUnitOfWork>();

            // references to control filters and sorts
            builder.Services.AddScoped<IPageHelper, PageHelper>();
            builder.Services.AddScoped<IContactFilters, ContactFilters>();
            builder.Services.AddScoped<GridQueryAdapter>();
            builder.Services.AddScoped<EditService>();

            // not used here but would be useful on the server
            builder.Services.AddScoped(sp =>
                new ClaimsPrincipal(new ClaimsIdentity()));

            await builder.Build().RunAsync();
        }
    }
}
