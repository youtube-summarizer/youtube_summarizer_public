using Microsoft.EntityFrameworkCore;
using YtbSummarizer.Api.Database;
using YtbSummarizer.Api.Interfaces.Services;
using YtbSummarizer.Models.Entities;

namespace YtbSummarizer.Api.Services;

public class YoutubeSubscriptionService : IYoutubeSubscriptionsService
{
	private readonly IUoW _uow;

	public YoutubeSubscriptionService(IUoW uow)
	{
		_uow = uow;
	}

	public async Task<List<Subscription>> GetAllForUser(string userId)
	{
		return await _uow.Subscriptions.Where(sub => sub.UserId.Equals(userId)).ToListAsync();
	}

	public async Task<bool> AddSubscriptionAsync(string channelId, string userId)
	{
		if (await _uow.Subscriptions.AnyAsync(sub => sub.ChannelId.Equals(channelId) && sub.UserId.Equals(userId)))
			return true;
		await _uow.Subscriptions.AddAsync(new Subscription
		{
			ChannelId = channelId,
			UserId = userId
		});

		return await _uow.SaveChangesAsync() > 0;
	}

	public async Task<bool> RemoveSubscriptionAsync(string channelId, string userId)
	{
		var subscription = await _uow.Subscriptions.Where(sub => sub.ChannelId.Equals(channelId) && sub.UserId.Equals(userId)).FirstOrDefaultAsync();

		if (subscription == null)
			return false;

		_uow.Subscriptions.Remove(subscription);
		return await _uow.SaveChangesAsync() > 0;
	}

	public async Task<List<string>> GetAllChannelIdsAsync()
	{
		return await _uow.Subscriptions.Select(s => s.ChannelId).Distinct().ToListAsync();
	}
}