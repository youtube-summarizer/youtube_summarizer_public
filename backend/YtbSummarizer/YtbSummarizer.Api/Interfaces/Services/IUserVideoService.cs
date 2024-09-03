using YtbSummarizer.Models.DTOs;
using YtbSummarizer.Models.Entities;

namespace YtbSummarizer.Api.Interfaces.Services;

public interface IUserVideoService
{
    // Retrieves all videos associated with a specific userId.
    Task<List<Video>> GetAllVideosForUser(string userId);   
    // Adds a video to a user's list of videos.
    Task<bool> AddVideoForUserAsync(string videoId, string userId);
    // Removes a video from a user's list of videos.
    Task<bool> RemoveVideoForUserAsync(string videoId, string userId);
}