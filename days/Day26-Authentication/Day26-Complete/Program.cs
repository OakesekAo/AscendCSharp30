// Day 26 - Authentication & JWT
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplicationBuilder.CreateBuilder(args);
var key = Encoding.ASCII.GetBytes("your-256-bit-secret-key-here!");
builder.Services.AddAuthentication(x => { x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; })
    .AddJwtBearer(x => { x.TokenValidationParameters = new TokenValidationParameters { ValidateIssuerSigningKey = true, IssuerSigningKey = new SymmetricSecurityKey(key), ValidateIssuer = false, ValidateAudience = false }; });
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI(); }
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapPost("/login", (LoginReq r) => Results.Ok(new { token = "jwt-token-here", user = r.Username })).WithOpenApi();
app.MapGet("/protected", () => Results.Ok("Protected endpoint")).RequireAuthorization().WithOpenApi();
app.Run();

public record LoginReq(string Username, string Password);
