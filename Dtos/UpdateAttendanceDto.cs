public class UpdateAttendanceDto
{
    public string Date { get; set; } = default!;
    public string InTime { get; set; } = default!;
    public string OutTime { get; set; } = default!;
    public int Duration { get; set; }
}