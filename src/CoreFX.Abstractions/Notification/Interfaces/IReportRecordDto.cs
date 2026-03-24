using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoreFX.Abstractions.Notification.Interfaces
{
    public interface IReportRecordDto
    {
        int Sn { get; set; }
        string From { get; set; }
        string Category { get; set; }
        string What { get; set; }
        string Who { get; set; }
        string When { get; set; }
        int Count { get; set; }
        string Detail { get; set; }

        [JsonIgnore]
        bool IsSuccess { get; set; }

        [JsonIgnore]
        DateTime _ts { get; set; }

        [JsonIgnore]
        Dictionary<string, string> ExtMap { get; set; }
    }
}
