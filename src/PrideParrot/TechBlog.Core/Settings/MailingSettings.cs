namespace TechBlog.Core.Settings;

public class MailingSettings
{
	public const string ConfigSectionName = nameof(MailingSettings);

	public string SenderName { get; set; }

	public string SenderAddress { get; set; }

	public string ReceiverAddress { get; set; }

	public string MailingService { get; set; }
}