using System.Security.Cryptography;
using FluentValidation;
using SnowflakeGenerator;
using UrlShortener.Infrastructure.CassandraRepository;
using UrlShorter.Domain.Models;

namespace UrlShortener.Application.UseCases.CreateUrl;

public class CreateUrlUseCase : ICreateUrlUseCase
{
    private readonly IValidator<CreateUrlCommand> _validator;
    private readonly IRepository _repository;
    private readonly Settings _snowFlakeSettings;
    private readonly Base62Manager _base62Manager;

    public CreateUrlUseCase(
        IValidator<CreateUrlCommand> validator,
        IRepository repository,
        Settings snowFlakeSettings,
        Base62Manager base62Manager)
    {
        _validator = validator;
        _repository = repository;
        _snowFlakeSettings = snowFlakeSettings;
        _base62Manager = base62Manager;
    }
    public async Task<ShortUrl> ExecuteAsync(CreateUrlCommand command, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(command, cancellationToken);
        
        var newUrl = new ShortUrl
        {
            ShortCode = command.Alias ?? _base62Manager.ToBase62(
                                    new Snowflake(_snowFlakeSettings)
                                        .NextID())
                                        [..8],
            OriginalUrl = command.OriginalUrl,
            CreatedAt = DateTimeOffset.UtcNow,
            ExpiresAt = command.ExpiredAt,
            IsActive = true,
            Clicks = 0
        };

        await _repository.AddNewUrlAsync(newUrl, cancellationToken);
            
        return newUrl;
    }
}