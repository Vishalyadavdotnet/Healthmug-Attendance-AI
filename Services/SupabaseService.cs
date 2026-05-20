using System.Text;
using System.Text.Json;

public class SupabaseService
{
    private readonly HttpClient _http;

    public SupabaseService(IConfiguration config)
    {
        _http = new HttpClient();
        var url = config["Supabase:Url"] ?? throw new ArgumentNullException("Supabase:Url missing");
        var key = config["Supabase:Key"] ?? throw new ArgumentNullException("Supabase:Key missing");
        
        _http.BaseAddress = new Uri(url);

        _http.DefaultRequestHeaders.Add("apikey", key);
        _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {key}");
        _http.DefaultRequestHeaders.Add("Prefer", "return=representation");
        
        // 🔥 Attendance schema use karne ke liye headers
        _http.DefaultRequestHeaders.Add("Accept-Profile", "attendance");
        _http.DefaultRequestHeaders.Add("Content-Profile", "attendance");
    }

    // 🔥 raw json return karega
    public async Task<string> GetRaw(string url)
    {
        var res = await _http.GetAsync(url);
        return await res.Content.ReadAsStringAsync();
    }

    public async Task<string> Post(string url, object data)
    {
        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var res = await _http.PostAsync(url, content);
        return await res.Content.ReadAsStringAsync();
    }

    public async Task<string> Patch(string url, object data)
    {
        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage(new HttpMethod("PATCH"), url)
        {
            Content = content
        };

        var res = await _http.SendAsync(request);
        return await res.Content.ReadAsStringAsync();
    }

    public async Task<string> Delete(string url)
    {
        var res = await _http.DeleteAsync(url);
        return await res.Content.ReadAsStringAsync();
    }
}