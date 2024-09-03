using YtbSummarizer.Models.DTOs;
using YtbSummarizer.Models.Entities.Base;

namespace YtbSummarizer.Models.Entities;

public class Video : BaseEntity
{
	public string Summary { get; set; }
	public string Title { get; set; }
	public string YoutubeVideoId { get; set; }
	public string ChannelId { get; set; }
	public DateTime UploadedDate { get; set; }

	public static Video FromVideoModel(VideoModel videoModel)
	{
		return new Video
		{
			Title = videoModel.Title,
			YoutubeVideoId = videoModel.Id,
			UploadedDate = videoModel.PublishedAt,
			ChannelId = videoModel.ChannelId
		};
	}
}

