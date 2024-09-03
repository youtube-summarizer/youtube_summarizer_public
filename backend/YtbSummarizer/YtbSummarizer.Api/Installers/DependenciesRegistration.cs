using YtbSummarizer.Api.Authentication;
using YtbSummarizer.Api.Database;
using YtbSummarizer.Api.Interfaces.Services;
using YtbSummarizer.Api.Services;

namespace YtbSummarizer.Api.Installers;

public static class DependenciesRegistration
{
	public static void AddBusinessServices(this IServiceCollection services)
	{
		services.AddScoped<IUoW, UoW>();
		services.AddScoped<ApiKeyAuthFilter>();
		services.AddScoped<IYoutubeVideosService, YoutubeVideosService>();
		services.AddScoped<IYoutubeSubscriptionsService, YoutubeSubscriptionService>();
		services.AddScoped<IUserVideoService, UserVideoService>();
		services.AddScoped<IUserInteractedVideoService, UserInteractedVideoService>();
		services.AddSingleton<SqsService>();
	}
}