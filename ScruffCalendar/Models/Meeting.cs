using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ScruffCalendar.Models
{
    public class Meeting
    {
        /// <summary>
        /// Gets or sets the unqiue meeting ID.
        /// </summary>
        [JsonPropertyName("uuid")]
        public string Uuid { get; set; }

        /// <summary>
        /// Gets or sets the meeting ID.
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the user ID of the host of the meeting.
        /// </summary>
        [JsonPropertyName("host_id")]
        public string HostId { get; set; }

        /// <summary>
        /// Gets or sets the meeting topic.
        /// </summary>
        [JsonPropertyName("topic")]
        public string Topic { get; set; }

        /// <summary>
        /// Gets or sets the meeting status.
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the meeting start time in UTC.
        /// </summary>
        [JsonPropertyName("start_time")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the duration of the meeting.
        /// </summary>
        [JsonPropertyName("duration")]
        public long Duration { get; set; }

        /// <summary>
        /// Gets or sets the timezone to format the meeting start time.
        /// </summary>
        [JsonPropertyName("timezone")]
        public string Timezone { get; set; }

        /// <summary>
        /// Gets or sets the time the meeting was created.
        /// </summary>
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the Meeting URL that the host can use to start the meeting.
        /// </summary>
        [JsonPropertyName("start_url")]
        public string StartUrl { get; set; }

        /// <summary>
        /// Gets or sets the meeting URL that participants can use to join the meeting.
        /// </summary>
        [JsonPropertyName("join_url")]
        public string JoinUrl { get; set; }
    }
}
