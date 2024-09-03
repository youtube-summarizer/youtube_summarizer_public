namespace YtbSummarizer.Models.DTOs;

public class VideoModel
{
	public string Id { get; set; }
	public string Title { get; set; }
	public DateTime PublishedAt { get; set; }
	public string ChannelId { get; set; }
}