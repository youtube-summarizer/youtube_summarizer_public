using Cryptofy.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using YtbSummarizer.Api.Interfaces.Services;
using YtbSummarizer.Models.DTOs;

namespace YtbSummarizer.Api.Controllers;

public class UserInteractVideoController : BaseController
{

	private readonly IUserInteractedVideoService _userInteractedVideoService;

	public UserInteractVideoController(IUserInteractedVideoService userInteractedVideoService)
	{
		_userInteractedVideoService = userInteractedVideoService;
	}

	[HttpGet("all")]
	public async Task<IActionResult> GetAllForUser(string userId)
	{
		return Ok(await _userInteractedVideoService.GetUserVideoInteractions(userId));
	}

	[HttpPost("get")]
	public async Task<IActionResult> GetForUser(UserInteractedVideoModel userInteractedVideo)
	{
		return Ok(await _userInteractedVideoService.GetUserVideoInteraction(userInteractedVideo.UserId, userInteractedVideo.VideoId));
	}

	[HttpPost("edit")]
	public async Task<IActionResult> EditUserInteractedVideo(UserInteractedVideoModel userInteractedVideo)
	{
		return Ok(await _userInteractedVideoService.UpdateUserVideoInteraction(userInteractedVideo.UserId, userInteractedVideo.VideoId, userInteractedVideo.Score));
	}
}