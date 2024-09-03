using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using YtbSummarizer.Api.Database;
using YtbSummarizer.Api.Interfaces.Services;
using YtbSummarizer.Models.DTOs;
using YtbSummarizer.Models.Entities;

namespace YtbSummarizer.Api.Services;

public class UserInteractedVideoService : IUserInteractedVideoService
{
    private readonly IUoW _uow;

    public UserInteractedVideoService(IUoW uow)
    {
        _uow = uow;
    }
    
    public async Task<UserInteractedVideo> GetUserVideoInteraction(string userId, string videoId)
    {
        var userInteractedVideo = await _uow.UserInteractedVideos.Where(interactedVideo => interactedVideo.UserId.Equals(userId) && interactedVideo.VideoId.Equals(videoId)).FirstOrDefaultAsync();

        if (userInteractedVideo == null)
        {
            userInteractedVideo = new UserInteractedVideo { UserId = userId, VideoId = videoId, Score = null };
            await _uow.UserInteractedVideos.AddAsync(userInteractedVideo);
            await _uow.SaveChangesAsync();
        }

        return userInteractedVideo;
    }

    public async Task<List<UserInteractedVideo>> GetUserVideoInteractions(string userId)
    {
        return await _uow.UserInteractedVideos.Where(interactedVideo => interactedVideo.UserId.Equals(userId)).ToListAsync();
    }

    public async Task<bool> UpdateUserVideoInteraction(string userId, string videoId, int? score)
    {
        var userInteractedVideo = await _uow.UserInteractedVideos.Where(interactedVideo => interactedVideo.UserId.Equals(userId) && interactedVideo.VideoId.Equals(videoId)).FirstOrDefaultAsync();

        if (userInteractedVideo == null)
        {
            userInteractedVideo = new UserInteractedVideo { UserId = userId, VideoId = videoId, Score = score };
            await _uow.UserInteractedVideos.AddAsync(userInteractedVideo);
            await _uow.SaveChangesAsync();
            return true;
        }

        userInteractedVideo.Score = score;
        return await _uow.SaveChangesAsync() > 0;
    }

    public async Task<bool> RemoveInteractionsWithVideo(string videoId)
    {
        var userInteractedVideos = await _uow.UserInteractedVideos.Where(interactedVideo => interactedVideo.VideoId.Equals(videoId)).ToListAsync();

        if (userInteractedVideos.Count == 0) // there are no interactions with this video
            return true;

        _uow.UserInteractedVideos.RemoveRange(userInteractedVideos);
        return await _uow.SaveChangesAsync() > 0;
    }
  
}
