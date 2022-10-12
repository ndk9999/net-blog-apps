namespace TechBlog.Core.Settings;

public class RssSettings
{
	public const string ConfigSectionName = nameof(RssSettings);

	public string ChannelName { get; set; }

	public string Description { get; set; }

	public string Domain { get; set; }

	public string Copyright { get; set; }

	public string ContactEmail { get; set; }

	public int MaximumItems { get; set; }
}