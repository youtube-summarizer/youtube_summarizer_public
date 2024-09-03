using Cryptofy.Api.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YtbSummarizer.Api.Interfaces.Services;

namespace YtbSummarizer.Api.Controllers;

public class UserVideosController : BaseController
{
	private readonly IUserVideoService _userVideoService;
	public UserVideosController(IUserVideoService userVideoService)
	{
		_userVideoService = userVideoService;
	}

	[Authorize]
	[HttpGet("all")]
	public async Task<IActionResult> GetAllForUser(string userId)
	{
		return Ok(await _userVideoService.GetAllVideosForUser(userId));
	}

	[HttpPost("add")]
	public async Task<ActionResult<bool>> AddUserVideo(string userId, string videoId)
	{
		return Ok(await _userVideoService.AddVideoForUserAsync(videoId, userId));
	}

	[Authorize]
	[HttpDelete("delete")]
	public async Task<ActionResult<bool>> DeleteUserVideo(string userId, string videoId)
	{
		return Ok(await _userVideoService.RemoveVideoForUserAsync(videoId, userId));
	}
}
