using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SnowflakeGenerator;
using UrlShortener.Application.BackgroundServices;
using UrlShortener.Application.UseCases.CreateUrl;
using UrlShortener.Application.UseCases.DeleteUrl;
using UrlShortener.Application.UseCases.GetClicksInformation;
using UrlShortener.Application.UseCases.Redirect;
using UrlShortener.Application.UseCases.UpdateOriginalUrl;
using UrlShortener.Application.UseCases.UpdateUrlExpirationDate;

namespace UrlShortener.Application;

public static class ApplicationDependencyInjection
{
    public static void AddApplicationDependencies(this IServiceCollection services)
    {
        // for snowflake
        services.AddSingleton<Settings>(r => new Settings()
            {
                MachineIDBitLength = 8,
                CustomEpoch = new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero)
            })
            .AddSingleton<Base62Manager>();
        
        // validation
        services.AddValidatorsFromAssemblyContaining<RedirectCommandValidator>(includeInternalTypes: true);
        
        // use cases
        services
            .AddScoped<IRedirectUseCase, RedirectUseCase>()
            .AddScoped<ICreateUrlUseCase, CreateUrlUseCase>()
            .AddScoped<IDeleteUrlUseCase, DeleteUrlUseCase>()
            .AddScoped<IUpdateOriginalUrlUseCase, UpdateOriginalUrlUseCase>()
            .AddScoped<IUpdateUrlExpirationDate, UpdateUrlExpirationDate>()
            .AddScoped<IGetClicksInformation, GetClicksInformation>();
        
        // expiration check background service
        services.AddHostedService<UrlExpirationService>();
    }
}