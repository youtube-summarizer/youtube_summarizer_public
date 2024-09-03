using YtbSummarizer.Models.DTOs;
using YtbSummarizer.Models.Entities.Base;

namespace YtbSummarizer.Models.Entities;

public class UserVideos : BaseEntity
{
    public string UserId { get; set; }
    public string VideoId { get; set; } // This is the YoutubeVideoId
}