using YtbSummarizer.Models.Entities;

namespace YtbSummarizer.Api.Interfaces.Services;

public interface IYoutubeSubscriptionsService
{
	// Retrieves all subscriptions for a specific user.
	Task<List<Subscription>> GetAllForUser(string userId);
	// Adds a subscription for a specific user.
	Task<bool> AddSubscriptionAsync(string channelId, string userId);
	// Removes a subscription for a specific user.
	Task<bool> RemoveSubscriptionAsync(string channelId, string userId);
	// Retrieves all channel IDs that users are subscribed to.
	Task<List<string>> GetAllChannelIdsAsync();
}