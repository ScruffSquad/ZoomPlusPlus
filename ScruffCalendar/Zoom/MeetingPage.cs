using System;
using System.Text.Json.Serialization;
using ScruffCalendar.Models;

namespace ScruffCalendar.Zoom
{
    public class MeetingPage
    {
        [JsonPropertyName("page_count")]
        public long PageCount { get; set; }

        [JsonPropertyName("page_number")]
        public long PageNumber { get; set; }

        [JsonPropertyName("page_size")]
        public long PageSize { get; set; }

        [JsonPropertyName("total_records")]
        public long TotalRecords { get; set; }

        [JsonPropertyName("meetings")]
        public Meeting[] Meetings { get; set; }
    }
}
