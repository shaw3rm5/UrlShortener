using FluentValidation;
using UrlShortener.Application.Exceptions;

namespace UrlShortener.Application.UseCases.UpdateOriginalUrl;

public class UpdateOriginalUrlCommandValidator : AbstractValidator<UpdateOriginalUrlCommand>
{
    public UpdateOriginalUrlCommandValidator()
    {
        RuleFor(l => l.ShortUrl)
            .Length(8)
            .WithErrorCode(ValidationError.Invalid);
        RuleFor(r => r.OriginalUrl)
            .Matches("^(https?:\\/\\/)?([\\w\\-]+\\.)+[\\w\\-]{2,}([\\/\\w\\-.?=&%+]*)?$")
            .WithErrorCode(ValidationError.Invalid);
    }
}