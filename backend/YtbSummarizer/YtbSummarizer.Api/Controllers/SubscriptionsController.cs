using Cryptofy.Api.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YtbSummarizer.Api.Authentication;
using YtbSummarizer.Api.Interfaces.Services;
using YtbSummarizer.Models.Entities;

namespace YtbSummarizer.Api.Controllers;

public class SubscriptionsController : BaseController
{
	private readonly IYoutubeSubscriptionsService _youtubeSubscriptionsService;
	public SubscriptionsController(IYoutubeSubscriptionsService youtubeSubscriptionsService)
	{
		_youtubeSubscriptionsService = youtubeSubscriptionsService;
	}

	[Authorize]
	[HttpGet("all")]
	public async Task<IActionResult> GetAllForUser(string userId)
	{
		return Ok(await _youtubeSubscriptionsService.GetAllForUser(userId));
	}

	[HttpPost("add")]
	public async Task<ActionResult<bool>> AddSubscription(Subscription subscription)
	{
		return Ok(await _youtubeSubscriptionsService.AddSubscriptionAsync(subscription.ChannelId, subscription.UserId));
	}

	[HttpPost("delete")]
	public async Task<ActionResult<bool>> DeleteSubscription(Subscription subscription)
	{
		return Ok(await _youtubeSubscriptionsService.RemoveSubscriptionAsync(subscription.ChannelId, subscription.UserId));
	}

	[HttpGet("all-channel-ids")]
	[ServiceFilter(typeof(ApiKeyAuthFilter))]
	public async Task<IActionResult> GetAllChannelIds()
	{
		return Ok(await _youtubeSubscriptionsService.GetAllChannelIdsAsync());
	}
}
