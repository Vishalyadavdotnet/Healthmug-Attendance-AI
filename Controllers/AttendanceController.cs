using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

[ApiController]
[Route("api")]
public class AttendanceController : ControllerBase
{
    private readonly SupabaseService _supabase;

    private readonly JsonSerializerOptions _options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    public AttendanceController(SupabaseService supabase)
    {
        _supabase = supabase;
    }

    // CREATE
    [HttpPost("attendance")]
    public async Task<IActionResult> Create([FromBody] Attendance a)
    {
        var json = await _supabase.Post("/rest/v1/attendance", new
        {
            user_id = a.UserId,
            date = a.Date,
            in_time = a.InTime,
            out_time = a.OutTime,
            duration = a.Duration
        });

        var list = JsonSerializer.Deserialize<List<AttendanceDb>>(json, _options);
        var item = list?.FirstOrDefault();

        if (item == null)
            return BadRequest("Insert failed");

        return Ok(new
        {
            id = item.Id,
            userId = item.UserId,
            date = item.Date,
            inTime = item.InTime,
            outTime = item.OutTime,
            duration = item.Duration,
            shortLeaveMins = item.ShortLeaveMins,
            shortLeaveType = item.ShortLeaveType
        });
    }

    // GET
    [HttpGet("attendance/{userId}")]
    public async Task<IActionResult> Get(int userId)
    {
        var json = await _supabase.GetRaw($"/rest/v1/attendance?user_id=eq.{userId}");

        var list = JsonSerializer.Deserialize<List<AttendanceDb>>(json, _options);

        if (list == null)
            return Ok(new List<object>());

        return Ok(list.Select(a => new
        {
            id = a.Id,
            userId = a.UserId,
            date = a.Date,
            inTime = a.InTime,
            outTime = a.OutTime,
            duration = a.Duration,
            shortLeaveMins = a.ShortLeaveMins,
            shortLeaveType = a.ShortLeaveType
        }));
    }


[HttpGet("ping")]
[HttpHead("ping")]
public IActionResult Ping()
{
    return Ok("alive");
}
    // UPDATE
    [HttpPut("attendance/{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAttendanceDto dto)
    {
        var data = new
        {
            date = dto.Date,
            in_time = dto.InTime,
            out_time = dto.OutTime,
            duration = dto.Duration
        };

        var json = await _supabase.Patch($"/rest/v1/attendance?id=eq.{id}", data);

        var list = JsonSerializer.Deserialize<List<AttendanceDb>>(json, _options);
        var item = list?.FirstOrDefault();

        if (item == null)
            return NotFound();

        return Ok(new
        {
            id = item.Id,
            userId = item.UserId,
            date = item.Date,
            inTime = item.InTime,
            outTime = item.OutTime,
            duration = item.Duration,
            shortLeaveMins = item.ShortLeaveMins,
            shortLeaveType = item.ShortLeaveType
        });
    }

    // DELETE
    [HttpDelete("attendance/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _supabase.Delete($"/rest/v1/attendance?id=eq.{id}");
        return Ok("Deleted");
    }

    // APPLY LEAVE
    [HttpPost("attendance/apply-leave")]
    public async Task<IActionResult> ApplyLeave([FromBody] ApplyLeaveDto dto)
    {
        var data = new
        {
            short_leave_mins = dto.Hours * 60,
            short_leave_type = dto.Type
        };

        var json = await _supabase.Patch(
            $"/rest/v1/attendance?id=eq.{dto.AttendanceId}", data);

        var list = JsonSerializer.Deserialize<List<AttendanceDb>>(json, _options);
        var item = list?.FirstOrDefault();

        if (item == null)
            return BadRequest("Attendance not found");

        return Ok(new
        {
            id = item.Id,
            userId = item.UserId,
            date = item.Date,
            inTime = item.InTime,
            outTime = item.OutTime,
            duration = item.Duration,
            shortLeaveMins = item.ShortLeaveMins,
            shortLeaveType = item.ShortLeaveType
        });
    }
}
