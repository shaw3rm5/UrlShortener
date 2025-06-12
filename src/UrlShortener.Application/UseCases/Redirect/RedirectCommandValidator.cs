using FluentValidation;
using UrlShortener.Application.Exceptions;

namespace UrlShortener.Application.UseCases.Redirect;

public class RedirectCommandValidator : AbstractValidator<RedirectCommand>
{
    public RedirectCommandValidator()
    {
        RuleFor(r => r.ShortUrl)
            .NotNull()
            .WithErrorCode(ValidationError.Empty);
        RuleFor(r => r.ShortUrl)
            .Length(8)
            .WithErrorCode(ValidationError.Invalid);

    }
}