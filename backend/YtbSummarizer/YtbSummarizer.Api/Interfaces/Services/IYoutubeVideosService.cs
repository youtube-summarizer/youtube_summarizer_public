using YtbSummarizer.Models.DTOs;
using YtbSummarizer.Models.Entities;

namespace YtbSummarizer.Api.Interfaces.Services;

public interface IYoutubeVideosService
{
	// Retrieves all videos.
	Task<List<Video>> GetAllAsync();
	// Adds a new video.
	Task<bool> AddVideoAsync(VideoModel video);
	// Removes a video by its ID.
	Task<bool> RemoveVideoAsync(string videoId);
	// Updates an existing video.
	Task<bool> UpdateVideoAsync(Video video);
	// Updates the summary of a video.
	Task<bool> UpdateSummaryAsync(string videoId, string summary);
	// Checks if a video is saved by its YouTube ID.
	Task<bool> IsVideoSavedAsync(string youtubeVideoId);
	// Adds a single video for a specific user.
	Task AddSingleVideoAsync(string youtubeVideoId, string userId);
	// Regenerates a video by its YouTube ID.
	Task RegenerateVideoAsync(string youtubeVideoId);
}