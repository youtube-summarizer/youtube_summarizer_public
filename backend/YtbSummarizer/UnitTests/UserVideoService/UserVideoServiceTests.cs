using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using YtbSummarizer.Api.Database;
using YtbSummarizer.Api.Interfaces.Services;
using YtbSummarizer.Models.Entities;

namespace UnitTests.UserVideoService;

[TestClass]
public class UserVideoServiceTests
{
	private IUoW _uoW;
	private IUserVideoService _sut;
	private readonly string USER_ID = "userId";
	private const string USER_ID2 = "userId2";
	private const string VIDEO_ID = "videoId";
	private const string VIDEO_ID2 = "videoId2";


	[TestInitialize]
	public void Initialize()
	{
		var options = new DbContextOptionsBuilder<UoW>()
		   .UseInMemoryDatabase(databaseName: "Test", b => b.EnableNullChecks(false))
		   .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
		   .Options;

		_uoW = new UoW(options);
		_sut = new YtbSummarizer.Api.Services.UserVideoService(_uoW);
	}

	[TestCleanup]
	public async Task Cleanup()
	{
		await (_uoW as UoW).Database.EnsureDeletedAsync();
	}

	[TestMethod]
	public async Task WhenEmptyDb_ThenReturnNone()
	{
		// Arrange

		// Act
		var result = await _sut.GetAllVideosForUser(USER_ID);

		// Assert
		Assert.IsFalse(result.Any());
	}

	[TestMethod]
	public async Task WhenUserHasVideos_ThenReturnUserVideos()
	{
		// Arrange
		_uoW.UserVideos.Add(new UserVideos { UserId = USER_ID, VideoId = VIDEO_ID });
		_uoW.UserVideos.Add(new UserVideos { UserId = USER_ID, VideoId = VIDEO_ID2 });
		_uoW.Videos.Add(new Video { YoutubeVideoId = VIDEO_ID });
		_uoW.Videos.Add(new Video { YoutubeVideoId = VIDEO_ID2 });
		_uoW.SaveChanges();

		// Act
		var result = await _sut.GetAllVideosForUser(USER_ID);

		// Assert
		Assert.IsTrue(result.Count == 2);
	}

	[TestMethod]
	public async Task WhenUserDoesNotHaveVideos_And_AnotherUserHasVideos_ThenReturnNone()
	{
		// Arrange
		_uoW.UserVideos.Add(new UserVideos { UserId = USER_ID, VideoId = VIDEO_ID });
		_uoW.UserVideos.Add(new UserVideos { UserId = USER_ID, VideoId = VIDEO_ID2 });
		_uoW.Videos.Add(new Video { YoutubeVideoId = VIDEO_ID });
		_uoW.Videos.Add(new Video { YoutubeVideoId = VIDEO_ID2 });
		_uoW.SaveChanges();

		// Act
		var result = await _sut.GetAllVideosForUser(USER_ID2);

		// Assert
		Assert.IsFalse(result.Any());
	}
}