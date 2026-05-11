using System.Text.Json.Serialization;

public class Attendance
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Date { get; set; }

    public string InTime { get; set; }

    public string OutTime { get; set; }

    public int Duration { get; set; }

    public int? ShortLeaveMins { get; set; }

    public string? ShortLeaveType { get; set; }
}