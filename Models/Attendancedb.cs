using System.Text.Json.Serialization;

public class AttendanceDb
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    [JsonPropertyName("date")]
    public string Date { get; set; }

    [JsonPropertyName("in_time")]
    public string InTime { get; set; }

    [JsonPropertyName("out_time")]
    public string OutTime { get; set; }

    [JsonPropertyName("duration")]
    public int Duration { get; set; }

    [JsonPropertyName("short_leave_mins")]
    public int? ShortLeaveMins { get; set; }

    [JsonPropertyName("short_leave_type")]
    public string? ShortLeaveType { get; set; }
}