using Microsoft.EntityFrameworkCore;
using YtbSummarizer.Models.Entities;

namespace YtbSummarizer.Api.Database;

public interface IUoW : IDisposable
{
	DbSet<Video> Videos { get; set; }
	DbSet<UserVideos> UserVideos { get; set; }
	DbSet<Subscription> Subscriptions { get; set; }
	DbSet<UserInteractedVideo> UserInteractedVideos { get; set; }

	Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess = true, CancellationToken cancellationToken = default);
	int SaveChanges();
}