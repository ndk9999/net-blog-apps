using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TatBlog.Core.Entities;
using TatBlog.Core.Settings;
using TatBlog.Services.Timing;

namespace TatBlog.Services.Security;

public class JwtTokenGenerator : IJwtTokenGenerator
{
	private readonly ITimeProvider _timeProvider;
	private readonly JwtSettings _settings;

	public JwtTokenGenerator(
		ITimeProvider timeProvider, 
		IOptions<JwtSettings> jwtOptions)
	{
		_timeProvider = timeProvider;
		_settings = jwtOptions.Value;
	}

	public string GenerateToken(string username)
	{
		var signingCredentials = new SigningCredentials(
			new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret)),
			SecurityAlgorithms.HmacSha256Signature);

		var claims = new[]
		{
			new Claim(ClaimTypes.NameIdentifier, username),
			new Claim(ClaimTypes.Name, username),
			new Claim(ClaimTypes.Email, username + "@gmail.com"),
			new Claim(ClaimTypes.Role, "Administrator"),
		};

		var securityToken = new JwtSecurityToken(
			issuer: _settings.Issuer,
			audience: _settings.Audience,
			expires: _timeProvider.Now.AddMinutes(_settings.ExpiryMinutes),
			claims: claims,
			signingCredentials: signingCredentials);

		return new JwtSecurityTokenHandler().WriteToken(securityToken);
	}
}