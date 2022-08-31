using System;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace FakeAPI.Api;

public class Startup
{
    public IConfiguration Configuration { get; }
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        var appSettings = new AppSettings();
        Configuration.Bind(appSettings);

        services.AddControllers();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Test API", Version = "v1" });
        });

        services.AddScoped<IAppDataService, AppDataService>();
        services.AddScoped<IUserDataService, UserDataService>();
        services.AddScoped<IDataContainer, DataContainer>();
        services.AddScoped(x =>
        {
            var cosmosClient = new CosmosClient(
                appSettings.CosmosConnection,
                new CosmosClientOptions
                {
                    SerializerOptions = new CosmosSerializationOptions
                    {
                        IgnoreNullValues = true,
                        PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                    }
                });

            return cosmosClient.GetContainer(appSettings.DatabaseId, appSettings.DatabaseContainerId);
        });

        services.AddScoped(x =>
        {
            var client = new SecretClient(
                new Uri(appSettings.KeyVault.Uri),
            new ClientSecretCredential(
                appSettings.KeyVault.TennantId,
                appSettings.KeyVault.ApplicationId,
                appSettings.KeyVault.Secret));
            return client;
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();

            endpoints.Map(
                EndpointsMapper.RoutePath,
                EndpointsMapper.Request);
        });

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("v1/swagger.json", "fakeapi");
        });
    }
}