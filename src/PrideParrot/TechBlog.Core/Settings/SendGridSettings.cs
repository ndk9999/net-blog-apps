namespace TechBlog.Core.Settings;

public class SendGridSettings
{
	public const string ConfigSectionName = nameof(SendGridSettings);

	public string ApiKey { get; set; }
}