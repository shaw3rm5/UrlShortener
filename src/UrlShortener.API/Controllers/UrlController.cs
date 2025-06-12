using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.UseCases.CreateUrl;
using UrlShortener.Application.UseCases.DeleteUrl;
using UrlShortener.Application.UseCases.GetClicksInformation;
using UrlShortener.Application.UseCases.Redirect;
using UrlShortener.Application.UseCases.UpdateOriginalUrl;
using UrlShortener.Application.UseCases.UpdateUrlExpirationDate;
using UrlShorter.Domain.Models;

namespace UrlShortener.API.Controllers;


[ApiController]
public class UrlController : ControllerBase
{
    [HttpGet("{shortUrl}")]
    public async Task<IActionResult> RedirectToOriginalUrl(
        string shortUrl,
        [FromServices] IRedirectUseCase useCase,
        CancellationToken cancellationToken)
    {
        var command = new RedirectCommand(shortUrl);
        var urlClick = new UrlClick
        {
            ShortCode = command.ShortUrl,
            IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty,
            UserAgent = HttpContext.Request.Headers.UserAgent.ToString(),
        };
        var resultLink = await useCase.ExecuteAsync(command, urlClick, cancellationToken);
        return Redirect(resultLink);
    }

    [HttpGet("clicks/{shortUrl}")]
    public async Task<IActionResult> GetClicks(
        string shortUrl,
        [FromServices] IGetClicksInformation useCase,
        CancellationToken cancellationToken)
    {
        var command = new GetClicksInformationCommand(shortUrl);
        var (collection, count) = await useCase.ExecuteAsync(command, cancellationToken);
        return Ok(new
        {
            collection,
            count
        });
    }
    
    [HttpPost("createUrl")]
    public async Task<IActionResult> CreateNewShortUrl(
        [FromBody] CreateUrlCommand command,
        [FromServices] ICreateUrlUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.ExecuteAsync(command, cancellationToken);
        return Ok(new
        {
            result
        });
    }

    [HttpDelete("deleteUrl/{shortUrl}")]
    public async Task<IActionResult> DeleteUrl(
        string shortUrl,
        [FromServices] IDeleteUrlUseCase useCase,
        CancellationToken cancellationToken)
    {
        var command = new DeleteUrlCommand(shortUrl);
        await useCase.ExecuteAsync(command, cancellationToken);
        return Ok(shortUrl);
    }

    [HttpPost("updateOriginalUrl")]
    public async Task<IActionResult> UpdateOriginalUrl(
        [FromBody] UpdateOriginalUrlCommand command,
        [FromServices] IUpdateOriginalUrlUseCase useCase,
        CancellationToken cancellationToken
    )
    {
        await useCase.ExecuteAsync(command, cancellationToken);
        return Ok(new { command.ShortUrl, command.OriginalUrl });
    }
    
    [HttpPost("updateUrlExpirationDate")]
    public async Task<IActionResult> UpdateUrlExpirationDate(
        [FromBody] UpdateUrlExpirationDateCommand command,
        [FromServices] IUpdateUrlExpirationDate useCase,
        CancellationToken cancellationToken
    )
    {
        await useCase.ExecuteAsync(command, cancellationToken);
        return Ok();
    }

}
