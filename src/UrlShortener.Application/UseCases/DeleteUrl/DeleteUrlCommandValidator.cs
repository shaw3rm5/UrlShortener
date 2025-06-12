using FluentValidation;
using UrlShortener.Application.Exceptions;

namespace UrlShortener.Application.UseCases.DeleteUrl;

public class DeleteUrlCommandValidator : AbstractValidator<DeleteUrlCommand>
{
    public DeleteUrlCommandValidator()
    {
        RuleFor(x => x.ShortUrl)
            .NotNull().NotEmpty()
            .WithErrorCode(ValidationError.Empty);
        RuleFor(x => x.ShortUrl)
            .Length(8)
            .WithErrorCode(ValidationError.Invalid);
    }
}