using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using YtbSummarizer.Models.ServerModels;

namespace YtbSummarizer.Api.Installers;

public static class AwsCognitoInstaller
{
	public static void InstallCognito(this IServiceCollection services, IConfiguration configuration)
	{
		var cognitoSettings = configuration.GetSection("Aws:Cognito").Get<CognitoSettings>();
		if (cognitoSettings is null)
		{
			throw new ArgumentNullException(nameof(cognitoSettings));
		}

		services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(options =>
			{
				options.Audience = cognitoSettings.AppClientId;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidIssuer = $"https://cognito-idp.{cognitoSettings.Region}.amazonaws.com/{cognitoSettings.UserPoolId}",
					ValidateAudience = true,
					ValidAudience = cognitoSettings.AppClientId,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					IssuerSigningKeyResolver = (_, _, _, parameters) =>
					{
						using var httpClient = new HttpClient();
						var jwksUri = new Uri(parameters.ValidIssuer + "/.well-known/jwks.json");
						var json = httpClient.GetStringAsync(jwksUri).Result;
						return new JsonWebKeySet(json).Keys;
					}
				};
			});
	}
}