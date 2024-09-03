using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YtbSummarizer.Models.DTOs;

public class UserInteractedVideoModel
{
    public string UserId { get; set; }
    public string VideoId { get; set; }
    public int? Score { get; set; }
}