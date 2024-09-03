using YtbSummarizer.Models.DTOs;

namespace YtbSummarizer.CheckerWorker.Interfaces;

public interface IApiService
{
	Task<List<string>> GetAllYoutubeChannelIdsAsync();
	Task<bool> IsVideoSavedAsync(string videoId);
	Task<bool> AddVideoAsync(VideoModel video);
}