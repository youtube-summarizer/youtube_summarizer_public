using YtbSummarizer.Models.DTOs;
using YtbSummarizer.Models.Entities.Base;

namespace YtbSummarizer.Models.Entities;

public class Subscription : BaseEntity
{
    public string ChannelId { get; set; }
    public string ChannelTitle { get; set; }
    public string UserId { get; set; }
}