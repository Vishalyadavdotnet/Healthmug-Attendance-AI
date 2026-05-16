public class RegisterDto
{
    public string PhoneNumber { get; set; } = default!;
    public string Pin { get; set; } = default!;
    public string ShiftHours { get; set; } = default!;
    public decimal Salary { get; set; }
    public int ClHours { get; set; }
    public int ElHours { get; set; }
    public string SaturdayRule { get; set; } = default!;
}