using Google.Apis.YouTube.v3;
using YtbSummarizer.CheckerWorker.Interfaces;
using YtbSummarizer.CheckerWorker.Services;
using YtbSummarizer.Models.DTOs;

namespace YtbSummarizer.CheckerWorker
{
	public class Worker : BackgroundService
	{
		private readonly ILogger<Worker> _logger;
		private readonly YouTubeService _youtubeService;
		private readonly SqsService _sqsService;
		private readonly IApiService _apiService;

		public Worker(ILogger<Worker> logger, YouTubeService youtubeService, SqsService sqsService,
			IApiService apiService)
		{
			_logger = logger;
			_youtubeService = youtubeService;
			_sqsService = sqsService;
			_apiService = apiService;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			var subscriberIds = await _apiService.GetAllYoutubeChannelIdsAsync();

			while (!stoppingToken.IsCancellationRequested)
			{
				_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

				var newVideos = new List<VideoModel>();

				foreach (var subscriberId in subscriberIds)
				{
					var fetchedVideos = await GetLatestVideosAsync(subscriberId, stoppingToken);

					foreach (var fetchedVideo in fetchedVideos)
					{
						var isSaved = await _apiService.IsVideoSavedAsync(fetchedVideo.Id);
						if (!isSaved)
						{
							newVideos.Add(fetchedVideo);
						}
					}
				}

				newVideos.Sort((x, y) => x.PublishedAt.CompareTo(y.PublishedAt));

				foreach (var newVideo in newVideos)
				{
					await _apiService.AddVideoAsync(newVideo);
					await _sqsService.SendMessageAsync(newVideo.Id);
				}
				newVideos.Clear();

				await Task.Delay(600000, stoppingToken);
			}
		}

		private async Task<List<VideoModel>> GetLatestVideosAsync(string channelId, CancellationToken stoppingToken)
		{
			var searchListRequest = _youtubeService.Search.List("snippet");
			searchListRequest.ChannelId = channelId;
			searchListRequest.Order = SearchResource.ListRequest.OrderEnum.Date;
			searchListRequest.MaxResults = 1;

			var result = new List<VideoModel>();

			try
			{
				var searchListResponse = await searchListRequest.ExecuteAsync(stoppingToken);

				foreach (var searchResult in searchListResponse.Items)
				{
					if (searchResult.Id.Kind != "youtube#video")
					{
						continue;
					}

					var publishedAt = searchResult.Snippet?.PublishedAtDateTimeOffset;
					var publishedDateUtc = publishedAt?.UtcDateTime ?? DateTime.MinValue;

					_logger.LogInformation(
						$"Channel ID: {channelId}, Video Title: {searchResult.Snippet?.Title}, Video ID: {searchResult.Id.VideoId}, Uploaded Date (UTC): {publishedDateUtc}");

					result.Add(new VideoModel
					{
						Id = searchResult.Id.VideoId,
						Title = searchResult.Snippet?.Title,
						ChannelId = channelId,
						PublishedAt = publishedDateUtc
					});
				}
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error fetching videos for channel {channelId}: {ex.Message}");
			}

			return result;
		}
	}
}
