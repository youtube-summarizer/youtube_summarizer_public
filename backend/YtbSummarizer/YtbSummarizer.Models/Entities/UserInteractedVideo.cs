using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YtbSummarizer.Models.Entities.Base;

namespace YtbSummarizer.Models.Entities;

/*
GET /all ?userId={}&videoId={}
POST /add UserInteractedVideo
POST/PUT /edit UserInteractedVideo
*/

public class UserInteractedVideo : BaseEntity
{
    public string UserId { get; set; }
    public string VideoId { get; set; }

    // by default null, max 5 
    [Range(1, 5)]
    public int? Score { get; set; }
}