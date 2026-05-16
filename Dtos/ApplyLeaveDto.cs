public class ApplyLeaveDto
{
    public int AttendanceId { get; set; }
    public string Type { get; set; } = default!;
    public int Hours { get; set; }
}