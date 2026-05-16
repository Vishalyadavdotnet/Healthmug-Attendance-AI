public class UpdateUserDto
{
    public string ShiftHours { get; set; } = default!;
    public decimal Salary { get; set; }
    public int ClHours { get; set; }
    public int ElHours { get; set; }
    public string SaturdayRule { get; set; } = default!;
}