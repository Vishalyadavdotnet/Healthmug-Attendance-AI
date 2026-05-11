using System.Text;
using System.Text.Json;

public class SupabaseService
{
    private readonly HttpClient _http;

    public SupabaseService()
    {
        _http = new HttpClient();
        _http.BaseAddress = new Uri("https://gnzlwcurrpudbnernjdu.supabase.co");

        _http.DefaultRequestHeaders.Add("apikey", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Imduemx3Y3VycnB1ZGJuZXJuamR1Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NzQzNDM1OTUsImV4cCI6MjA4OTkxOTU5NX0.2Y0tEOSit0UPwc_gfWy23dL_qLROASyFNvnoJRE70KM");
        _http.DefaultRequestHeaders.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Imduemx3Y3VycnB1ZGJuZXJuamR1Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NzQzNDM1OTUsImV4cCI6MjA4OTkxOTU5NX0.2Y0tEOSit0UPwc_gfWy23dL_qLROASyFNvnoJRE70KM");
        _http.DefaultRequestHeaders.Add("Prefer", "return=representation");
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