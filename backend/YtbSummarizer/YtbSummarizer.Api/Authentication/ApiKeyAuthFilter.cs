using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using YtbSummarizer.Models.Constants;

namespace YtbSummarizer.Api.Authentication;

public class ApiKeyAuthFilter : IAuthorizationFilter
{
	private readonly IConfiguration _configuration;

	public ApiKeyAuthFilter(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public void OnAuthorization(AuthorizationFilterContext context)
	{
		// no api key provided
		if (!context.HttpContext.Request.Headers.TryGetValue(AuthConstants.API_KEY_HEADER_NAME, out var extractedApiKey))
		{
			context.Result = new UnauthorizedObjectResult("Api Key not provided");
			return;
		}

		var apiKey = _configuration.GetValue<string>(AuthConstants.API_KEY_SECTION_NAME);
		if (apiKey!.Equals(extractedApiKey))
		{
			return;
		}

		context.Result = new UnauthorizedObjectResult("Unauthorized. Invalid Api key");
	}
}