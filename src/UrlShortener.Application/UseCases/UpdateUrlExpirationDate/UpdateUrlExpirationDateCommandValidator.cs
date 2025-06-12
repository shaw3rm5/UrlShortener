using FluentValidation;
using UrlShortener.Application.Exceptions;

namespace UrlShortener.Application.UseCases.UpdateUrlExpirationDate;

public class UpdateUrlExpirationDateCommandValidator : AbstractValidator<UpdateUrlExpirationDateCommand>
{
    private readonly DateTimeOffset Now = DateTimeOffset.UtcNow;  
    public UpdateUrlExpirationDateCommandValidator()
    {
        RuleFor(r => r.ShortUrl)
            .Length(8)
            .WithErrorCode(ValidationError.Invalid);
        RuleFor(r => r.ExpirationDate)
            .NotNull()
            .Must(e => e > Now)
            .WithErrorCode(ValidationError.Invalid);
    }
}