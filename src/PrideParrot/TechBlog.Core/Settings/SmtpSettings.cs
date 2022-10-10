namespace TechBlog.Core.Settings;

public class SmtpSettings
{
	public const string ConfigSectionName = nameof(SmtpSettings);

	public string Host { get; set; }

	public int Port { get; set; }

	public string Username { get; set; }

	public string Password { get; set; }

	public bool EnableSsl { get; set; }

	public bool UseDefaultCredentials { get; set; }

}