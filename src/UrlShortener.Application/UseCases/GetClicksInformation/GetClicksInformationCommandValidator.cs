using FluentValidation;
using UrlShortener.Application.Exceptions;

namespace UrlShortener.Application.UseCases.GetClicksInformation;

public class GetClicksInformationCommandValidator : AbstractValidator<GetClicksInformationCommand>
{
    public GetClicksInformationCommandValidator()
    {
        RuleFor(x => x.ShortCode)
            .NotNull().NotEmpty()
            .WithErrorCode(ValidationError.Empty);
        RuleFor(x => x.ShortCode)   
            .Length(8)
            .WithErrorCode(ValidationError.Invalid);
    }
}