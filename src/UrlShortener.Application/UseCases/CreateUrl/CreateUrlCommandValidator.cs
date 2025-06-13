using FluentValidation;
using UrlShortener.Application.Exceptions;

namespace UrlShortener.Application.UseCases.CreateUrl;

public class CreateUrlCommandValidator : AbstractValidator<CreateUrlCommand>
{
    private DateTimeOffset Now = DateTimeOffset.UtcNow;

    public CreateUrlCommandValidator()
    {
        RuleFor(r => r.OriginalUrl)
            .NotEmpty()
            .WithErrorCode(ValidationError.Empty);
        RuleFor(r => r.ExpiredAt)
            .Must(e => e != null && e.Value > Now)
            .WithErrorCode(ValidationError.Invalid);
        RuleFor(r => r.OriginalUrl)
            .Matches(
                "^(https?:\\/\\/)?([\\w\\-]+\\.)+[\\w\\-]{2,}([\\/\\w\\-.?=&%+]*)?$")
            .WithErrorCode(ValidationError.Invalid);
        RuleFor(x => x.Alias)
            .Length(8)
            .When(x => !string.IsNullOrWhiteSpace(x.Alias))
            .WithErrorCode(ValidationError.Invalid);
    }
    
}