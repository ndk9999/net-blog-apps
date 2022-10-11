using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using TechBlog.Core.Contracts;
using TechBlog.Core.Settings;
using TechBlog.Services.Security;

namespace TechBlog.Web.Providers;

public class GoogleRecaptchaProvider : ICaptchaProvider
{
	private readonly IHttpClientFactory _clientFactory;
	private readonly RecaptchaSettings _captchaSettings;

	public GoogleRecaptchaProvider(IHttpClientFactory clientFactory, IOptions<RecaptchaSettings> captchaSettings)
	{
		_clientFactory = clientFactory;
		_captchaSettings = captchaSettings.Value;
	}

	public async Task<bool> VerifyAsync(IRequireCaptcha captcha)
	{
		if (string.IsNullOrWhiteSpace(captcha.CaptchaToken)) return false;

		using var httpClient = _clientFactory.CreateClient();

		var dataToPost = new Dictionary<string, string>()
		{
			["secret"] = _captchaSettings.SecretKey,
			["response"] = captcha.CaptchaToken
		};

		var postContent = new FormUrlEncodedContent(dataToPost);
		var httpResponse = await httpClient.PostAsync(_captchaSettings.ApiEndpoint, postContent);

		if (!httpResponse.IsSuccessStatusCode) return false;

		var verificationResult = await httpResponse.Content.ReadFromJsonAsync<RecaptchaResponse>();
		
		return verificationResult?.Success == true && verificationResult.Score > 0.5;
	}

	private class RecaptchaResponse
	{
		public bool Success { get; set; }

		public float Score { get; set; }

		public string Action { get; set; }

		public string HostName { get; set; }

		[JsonPropertyName("challenge_ts")]
		public DateTime ChallengeTs { get; set; }

		[JsonPropertyName("error-codes")]
		public List<string> ErrorCodes { get; set; }
	}
}