using System.Text;
using System.Text.Json;
using YtbSummarizer.CheckerWorker.Interfaces;
using YtbSummarizer.Models.DTOs;

namespace YtbSummarizer.CheckerWorker.Services;

public class ApiService : IApiService
{
	private readonly HttpClient _httpClient;

	public ApiService(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<List<string>> GetAllYoutubeChannelIdsAsync()
	{
		var response = await _httpClient.GetAsync("subscriptions/all-channel-ids");
		if (!response.IsSuccessStatusCode)
		{
			throw new Exception("Failed to get all youtube channel ids");
		}

		var content = await response.Content.ReadAsStringAsync();
		return JsonSerializer.Deserialize<List<string>>(content) ?? Enumerable.Empty<string>().ToList();
	}

	public async Task<bool> IsVideoSavedAsync(string videoId)
	{
		var url = $"videos/is-saved?youtubeVideoId={Uri.EscapeDataString(videoId)}";
		var response = await _httpClient.GetAsync(url);

		if (!response.IsSuccessStatusCode)
		{
			throw new Exception("Failed to get all youtube channel ids");
		}

		var result = await response.Content.ReadAsStringAsync();
		return JsonSerializer.Deserialize<bool>(result);
	}

	public async Task<bool> AddVideoAsync(VideoModel video)
	{
		var json = JsonSerializer.Serialize(video);
		var content = new StringContent(json, Encoding.UTF8, "application/json");

		var response = await _httpClient.PostAsync("videos/add", content);
		if (!response.IsSuccessStatusCode)
		{
			throw new Exception("Failed to get all youtube channel ids");
		}

		var result = await response.Content.ReadAsStringAsync();
		return JsonSerializer.Deserialize<bool>(result);
	}
}