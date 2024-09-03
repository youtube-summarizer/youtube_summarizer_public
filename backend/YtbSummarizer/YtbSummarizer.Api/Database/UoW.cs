using Microsoft.EntityFrameworkCore;
using YtbSummarizer.Models.Entities;

namespace YtbSummarizer.Api.Database;

public class UoW : DbContext, IUoW
{
	public UoW(DbContextOptions<UoW> options) : base(options)
	{
	}

	public virtual DbSet<Video> Videos { get; set; }
	public virtual DbSet<UserVideos> UserVideos { get; set; }
	public virtual DbSet<Subscription> Subscriptions { get; set; }
	public virtual DbSet<UserInteractedVideo> UserInteractedVideos { get; set; }
}