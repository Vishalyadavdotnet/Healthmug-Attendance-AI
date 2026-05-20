using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AttendanceApi.Middleware;
using AttendanceApi.Configuration;

var builder = WebApplication.CreateBuilder(args);

// 1. Add Configuration and Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<SupabaseService>();

// 2. Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.WithOrigins("https://hrhealthmug.vercel.app", "http://localhost:5173", "http://localhost:5174")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials());
});

// 3. Configure Rate Limiting
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? httpContext.Request.Headers.Host.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 100, // max 100 requests
                Window = TimeSpan.FromMinutes(1) // per minute
            }));
    options.RejectionStatusCode = 429;
});

// 4. Configure OpenTelemetry
builder.Services.AddObservability(builder.Configuration);

// 5. Configure JWT Authentication (if using own JWT tokens or Supabase JWTs)
var jwtAudience = builder.Configuration["Jwt:Audience"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtSecretKey = builder.Configuration["Jwt:SecretKey"];
if (!string.IsNullOrEmpty(jwtAudience) && !string.IsNullOrEmpty(jwtIssuer) && !string.IsNullOrEmpty(jwtSecretKey))
{
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey));

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtIssuer,
                ValidateAudience = true,
                ValidAudience = jwtAudience,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key
            };
        });
    builder.Services.AddAuthorization();
}

var app = builder.Build();

// 1. Exception Handling
app.UseMiddleware<GlobalExceptionMiddleware>();

// 2. HTTPS Enforcement
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}
app.UseHttpsRedirection();

// 3. Rate Limiting & CORS
app.UseRateLimiter();
app.UseCors("AllowAll");

// 4. Static Files
app.UseDefaultFiles();
app.UseStaticFiles();

// 5. Swagger (Secure: Only in Development)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 6. Auth
app.UseAuthentication();
app.UseAuthorization();

// 7. Map Endpoints
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();