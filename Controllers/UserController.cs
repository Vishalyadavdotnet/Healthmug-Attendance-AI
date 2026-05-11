using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

[ApiController]
[Route("api")]
public class UserController : ControllerBase
{
    private readonly SupabaseService _supabase;

    public UserController(SupabaseService supabase)
    {
        _supabase = supabase;
    }

    [HttpGet("user/{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var json = await _supabase.GetRaw($"/rest/v1/users?id=eq.{id}");

        var list = JsonSerializer.Deserialize<List<User>>(json);

        if (list == null || list.Count == 0)
            return NotFound();

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

    [HttpPut("user/{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto dto)
    {
        var data = new
        {
            shift_hours = dto.ShiftHours,
            salary = dto.Salary,
            cl_hours = dto.ClHours,
            el_hours = dto.ElHours,
            saturday_rule = dto.SaturdayRule
        };

        var json = await _supabase.Patch($"/rest/v1/users?id=eq.{id}", data);

        var list = JsonSerializer.Deserialize<List<User>>(json);

        if (list == null || list.Count == 0)
            return NotFound();

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