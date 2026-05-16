using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly SupabaseService _supabase;

    public AuthController(SupabaseService supabase)
    {
        _supabase = supabase;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var json = await _supabase.GetRaw(
            $"/rest/v1/users?phone_number=eq.{dto.Phone}&pin=eq.{dto.Pin}");

        var list = JsonSerializer.Deserialize<List<User>>(json);

        if (list == null || list.Count == 0)
            return Unauthorized();

        var user = list[0];

        return Ok(new
        {
            id = user.Id,
            phoneNumber = user.PhoneNumber,
            pin = user.Pin,
            shiftHours = user.ShiftHours,
            salary = user.Salary,
            clHours = user.CLHours,
            elHours = user.ELHours,
            saturdayRule = user.SaturdayRule
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        // 🔥 check if user already exists
        var existingJson = await _supabase.GetRaw(
            $"/rest/v1/users?phone_number=eq.{dto.PhoneNumber}");

        var existingUsers = JsonSerializer.Deserialize<List<User>>(existingJson);

        if (existingUsers != null && existingUsers.Count > 0)
        {
            return BadRequest("User already exists. Please login with correct PIN.");
        }

        var data = new
        {
            phone_number = dto.PhoneNumber,
            pin = dto.Pin,
            shift_hours = dto.ShiftHours,
            salary = dto.Salary,
            cl_hours = dto.ClHours,
            el_hours = dto.ElHours,
            saturday_rule = dto.SaturdayRule
        };

        var json = await _supabase.Post("/rest/v1/users", data);

        var list = JsonSerializer.Deserialize<List<User>>(json);

        if (list == null || list.Count == 0)
            return StatusCode(500, "Insert failed");

        var user = list[0];

        return Ok(new
        {
            id = user.Id,
            phoneNumber = user.PhoneNumber,
            pin = user.Pin,
            shiftHours = user.ShiftHours,
            salary = user.Salary,
            clHours = user.CLHours,
            elHours = user.ELHours,
            saturdayRule = user.SaturdayRule
        });
    }
}