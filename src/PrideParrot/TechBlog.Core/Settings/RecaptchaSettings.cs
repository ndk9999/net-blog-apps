namespace TechBlog.Core.Settings;

public class RecaptchaSettings
{
	public const string ConfigSectionName = nameof(RecaptchaSettings);

	public string SiteKey { get; set; }

	public string SecretKey { get; set; }

	public string ApiEndpoint { get; set; }
}