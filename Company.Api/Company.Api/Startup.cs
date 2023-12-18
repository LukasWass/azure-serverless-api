using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

[assembly: FunctionsStartup(typeof(Company.Api.Startup))]

namespace Company.Api;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddSingleton<IOpenApiConfigurationOptions>(_ =>
        {
            var options = new OpenApiConfigurationOptions()
            {
                Info = new OpenApiInfo()
                {
                    Version = Environment.GetEnvironmentVariable("API_VERSION"),
                    Title = Environment.GetEnvironmentVariable("API_TITLE"),
                    Description = Environment.GetEnvironmentVariable("API_DESCRIPTION"),
                    TermsOfService = new Uri(Environment.GetEnvironmentVariable("API_TERMS_OF_SERVICE_URL") ?? ""),
                    Contact = new OpenApiContact()
                    {
                        Name = Environment.GetEnvironmentVariable("API_CONTACT_NAME"),
                        Email = Environment.GetEnvironmentVariable("API_CONTACT_EMAIL"),
                        Url = new Uri(Environment.GetEnvironmentVariable("API_CONTACT_URL") ?? "")
                    }
                }
            };

            return options;
        });
    }
}