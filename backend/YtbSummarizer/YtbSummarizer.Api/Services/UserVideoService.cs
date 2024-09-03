using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using YtbSummarizer.Api.Database;
using YtbSummarizer.Api.Interfaces.Services;
using YtbSummarizer.Models.DTOs;
using YtbSummarizer.Models.Entities;

namespace YtbSummarizer.Api.Services;

public class UserVideoService : IUserVideoService
{
    private readonly IUoW _uow;

    public UserVideoService(IUoW uow)
    {
        _uow = uow;
    }

    public async Task<List<Video>> GetAllVideosForUser(string userId)
    {
        return await _uow.UserVideos.Where(video => video.UserId.Equals(userId))
            .Join(_uow.Videos, userVideo => userVideo.VideoId, video => video.YoutubeVideoId, (userVideo, video) => video)
            .ToListAsync();
    }

    public async Task<bool> AddVideoForUserAsync(string userId, string videoId)
    {
        await _uow.UserVideos.AddAsync(new UserVideos { VideoId = videoId, UserId = userId });

        return await _uow.SaveChangesAsync() > 0;
    }

    public async Task<bool> RemoveVideoForUserAsync(string userId, string videoId)
    {
        var userVideo = await _uow.UserVideos.Where(userVideo => userVideo.VideoId.Equals(videoId) && userVideo.UserId.Equals(userId)).FirstOrDefaultAsync();

        if (userVideo == null)
            return false;

        _uow.UserVideos.Remove(userVideo);
        return await _uow.SaveChangesAsync() > 0;
    }
}
