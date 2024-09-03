using YtbSummarizer.Models.DTOs;
using YtbSummarizer.Models.Entities;

namespace YtbSummarizer.Api.Interfaces.Services;

public interface IUserInteractedVideoService
{
    Task<UserInteractedVideo> GetUserVideoInteraction(string userId, string videoId); // Get user interaction with video (if it's not there, create one)
    Task<List<UserInteractedVideo>> GetUserVideoInteractions(string userId); // Get all user interactions with videos
    Task<bool> UpdateUserVideoInteraction(string userId, string videoId, int? score); // Update user interaction with video
    Task<bool> RemoveInteractionsWithVideo(string videoId); // Remove all interactions with video
}