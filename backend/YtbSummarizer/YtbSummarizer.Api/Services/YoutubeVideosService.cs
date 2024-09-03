using Google.Apis.YouTube.v3;
using Microsoft.EntityFrameworkCore;
using YtbSummarizer.Api.Database;
using YtbSummarizer.Api.Interfaces.Services;
using YtbSummarizer.Models.DTOs;
using YtbSummarizer.Models.Entities;
using Video = YtbSummarizer.Models.Entities.Video;

namespace YtbSummarizer.Api.Services;

public class YoutubeVideosService : IYoutubeVideosService
{
	private readonly IUoW _uow;
	private readonly SqsService _sqsService;
	private readonly YouTubeService _youtubeService;


	public YoutubeVideosService(IUoW uow, SqsService sqsService, YouTubeService youtubeService)
	{
		_uow = uow;
		_sqsService = sqsService;
		_youtubeService = youtubeService;
	}

	public async Task<List<Video>> GetAllAsync()
	{
		return await _uow.Videos.ToListAsync();
	}

	public async Task<bool> AddVideoAsync(VideoModel video)
	{
		var addedVideo = (await _uow.Videos.AddAsync(Video.FromVideoModel(video))).Entity;
		var result = await _uow.SaveChangesAsync() > 0;

		var userSubscriptions = await _uow.Subscriptions.Where(sub => sub.ChannelId.Equals(video.ChannelId)).ToListAsync();
		foreach (var subscription in userSubscriptions)
		{
			_uow.UserVideos.Add(new UserVideos()
			{
				UserId = subscription.UserId,
				VideoId = addedVideo.YoutubeVideoId
			});
		}
		await _uow.SaveChangesAsync();

		return result;
	}

	public async Task<bool> RemoveVideoAsync(string youtubeVideoId)
	{
		var video = await _uow.Videos.Where(video => video.YoutubeVideoId.Equals(youtubeVideoId)).FirstOrDefaultAsync();

		if (video == null)
			return false;

		_uow.Videos.Remove(video);
		return await _uow.SaveChangesAsync() > 0;
	}

	public async Task<bool> UpdateVideoAsync(Video video)
	{
		var existingVideo = await _uow.Videos.FirstOrDefaultAsync(v => v.YoutubeVideoId.Equals(video.YoutubeVideoId));

		if (existingVideo == null)
			return false;

		existingVideo.Summary = video.Summary;
		existingVideo.Title = video.Title;
		existingVideo.ChannelId = video.ChannelId;

		_uow.Videos.Update(existingVideo);

		return await _uow.SaveChangesAsync() > 0;
	}

	public async Task<bool> IsVideoSavedAsync(string youtubeVideoId)
	{
		return await _uow.Videos.AnyAsync(v => v.YoutubeVideoId == youtubeVideoId);
	}

	public async Task AddSingleVideoAsync(string youtubeVideoId, string userId)
	{
		var (title, publishedAt) = GetVideoDetails(youtubeVideoId);

		await _uow.Videos.AddAsync(new Video()
		{
			YoutubeVideoId = youtubeVideoId,
			Title = title,
			UploadedDate = publishedAt
		});

		await _uow.UserVideos.AddAsync(new UserVideos()
		{
			UserId = userId,
			VideoId = youtubeVideoId
		});

		await _uow.SaveChangesAsync();

		await _sqsService.SendMessageAsync(youtubeVideoId);
	}

	public async Task RegenerateVideoAsync(string youtubeVideoId)
	{
		await _sqsService.SendMessageAsync(youtubeVideoId);
	}

	public async Task<bool> UpdateSummaryAsync(string videoId, string summary)
	{
		var video = await _uow.Videos.FirstOrDefaultAsync(v => v.YoutubeVideoId == videoId);
		if (video is null)
		{
			return false;
		}

		video.Summary = summary;

		return await _uow.SaveChangesAsync() > 0;
	}

	private (string, DateTime) GetVideoDetails(string videoId)
	{
		var request = _youtubeService.Videos.List("snippet");
		request.Id = videoId;
		var response = request.Execute();

		return (response.Items[0].Snippet.Title, response.Items[0].Snippet.PublishedAtDateTimeOffset!.Value.UtcDateTime);
	}
}