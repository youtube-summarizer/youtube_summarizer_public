using Cryptofy.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using YtbSummarizer.Api.Authentication;
using YtbSummarizer.Api.Interfaces.Services;
using YtbSummarizer.Models.DTOs;
using YtbSummarizer.Models.Entities;
using YtbSummarizer.Models.Requests;

namespace YtbSummarizer.Api.Controllers;

public class VideosController : BaseController
{
	private readonly IYoutubeVideosService _youtubeVideosService;
	public VideosController(IYoutubeVideosService youtubeVideosService)
	{
		_youtubeVideosService = youtubeVideosService;
	}

	[HttpGet("all")]
	public async Task<IActionResult> GetAll()
	{
		return Ok(await _youtubeVideosService.GetAllAsync());
	}

	[HttpPost("add")]
	public async Task<ActionResult<bool>> AddVideo(VideoModel video)
	{
		return Ok(await _youtubeVideosService.AddVideoAsync(video));
	}

	[HttpPost("edit")]
	public async Task<ActionResult<bool>> EditVideo(Video video)
	{
		return Ok(await _youtubeVideosService.UpdateVideoAsync(video));
	}

	[HttpPost("delete")]
	public async Task<ActionResult<bool>> DeleteVideo(string id)
	{
		return Ok(await _youtubeVideosService.RemoveVideoAsync(id));
	}

	[ServiceFilter(typeof(ApiKeyAuthFilter))]
	[HttpGet("is-saved")]
	public async Task<ActionResult<bool>> IsVideoSaved(string youtubeVideoId)
	{
		return await _youtubeVideosService.IsVideoSavedAsync(youtubeVideoId);
	}

	[ServiceFilter(typeof(ApiKeyAuthFilter))]
	[HttpPatch("update-summary")]
	public async Task<ActionResult<bool>> UpdateVideo(VideoUpdateRequest videoUpdateRequest)
	{
		return await _youtubeVideosService.UpdateSummaryAsync(videoUpdateRequest.VideoId, videoUpdateRequest.Summary);
	}

	[HttpGet("add-single/{videoId}/{userId}")]
	public async Task<IActionResult> AddSingleVideo(string videoId, string userId)
	{
		await _youtubeVideosService.AddSingleVideoAsync(videoId, userId);
		return Ok();
	}

	[HttpGet("regenerate/{videoId}")]
	public async Task<IActionResult> RegenerateVideoSummary(string videoId)
	{
		await _youtubeVideosService.RegenerateVideoAsync(videoId);
		return Ok();
	}
}