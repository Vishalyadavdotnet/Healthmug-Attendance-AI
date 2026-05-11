using System.Text.Json.Serialization;

public class User
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("phone_number")]
    public string PhoneNumber { get; set; }

    [JsonPropertyName("pin")]
    public string Pin { get; set; }

    [JsonPropertyName("shift_hours")]
    public string ShiftHours { get; set; }

    [JsonPropertyName("salary")]
    public decimal Salary { get; set; }

    [JsonPropertyName("cl_hours")]
    public int CLHours { get; set; }

    [JsonPropertyName("el_hours")]
    public int ELHours { get; set; }

    [JsonPropertyName("saturday_rule")]
    public string SaturdayRule { get; set; }
}