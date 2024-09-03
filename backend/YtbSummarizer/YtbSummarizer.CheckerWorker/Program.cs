using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using YtbSummarizer.CheckerWorker.Interfaces;
using YtbSummarizer.CheckerWorker.Services;
using YtbSummarizer.Models.Constants;
using YtbSummarizer.Models.ServerModels;

namespace YtbSummarizer.CheckerWorker
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var host = Host.CreateDefaultBuilder(args)
				.ConfigureServices((hostContext, services) =>
				{
					var youtubeSettings = hostContext.Configuration.GetSection("Google").Get<GoogleApiSettings>();
					services.AddSingleton(new YouTubeService(new BaseClientService.Initializer
					{
						ApiKey = youtubeSettings!.ApiKey,
						ApplicationName = youtubeSettings.ApplicationName
					}));

					services.AddSingleton<SqsService>();

					services.AddSingleton<IApiService, ApiService>(); //NOTE: This has to be above the HttpClient registration
					services.AddHttpClient<IApiService, ApiService>(client =>
					{
						var apiUrl = hostContext.Configuration.GetValue<string>("ApiUrl");
						client.BaseAddress = new Uri(apiUrl!);

						var apiKey = hostContext.Configuration.GetValue<string>(AuthConstants.API_KEY_SECTION_NAME);
						client.DefaultRequestHeaders.Add(AuthConstants.API_KEY_HEADER_NAME, apiKey);
					});


					services.AddHostedService<Worker>();
				})
				.Build();

			host.Run();
		}
	}
}