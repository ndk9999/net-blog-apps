using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TechBlog.Core.DTO;
using TechBlog.Core.Entities;
using TechBlog.Core.Settings;
using TechBlog.Services.Blogs;

namespace TechBlog.Services.Rss;

public class FeedProvider : IFeedProvider
{
	private readonly ILogger<FeedProvider> _logger;
	private readonly IBlogRepository _blogRepository;
	private readonly RssSettings _rssSettings;
	private readonly Func<Post, string> _urlGenerator;

	public FeedProvider(
		ILogger<FeedProvider> logger, 
		IBlogRepository blogRepository,
		IOptions<RssSettings> rssSettings, 
		Func<Post, string> urlGenerator)
	{
		_logger = logger;
		_blogRepository = blogRepository;
		_urlGenerator = urlGenerator;
		_rssSettings = rssSettings.Value;
	}

	public async Task<byte[]> CreateAsync(CancellationToken cancellationToken = default)
	{
		var feed = new SyndicationFeed(
			_rssSettings.ChannelName,
			_rssSettings.Description,
			new Uri(_rssSettings.Domain),
			"RSSUrl",
			DateTimeOffset.Now);

		feed.Copyright = new TextSyndicationContent($"{DateTime.Now.Year} {_rssSettings.Copyright}");
		feed.ElementExtensions.Add("webmaster", "", _rssSettings.ContactEmail);

		var posts = await GetPostsAsync(cancellationToken);
		feed.Items = GenerateFeedItems(posts);

		return SerializeFeed(feed);
	}

	private byte[] SerializeFeed(SyndicationFeed feed)
	{
		var settings = new XmlWriterSettings()
		{
			Encoding = Encoding.UTF8,
			NewLineHandling = NewLineHandling.Entitize,
			NewLineOnAttributes = true,
			Indent = true
		};

		using var stream = new MemoryStream();
		using var xmlWriter = XmlWriter.Create(stream, settings);
		var rssFormatter = new Rss20FeedFormatter(feed, false);

		rssFormatter.WriteTo(xmlWriter);
		xmlWriter.Flush();
		
		return stream.ToArray();
	}

	private List<SyndicationItem> GenerateFeedItems(IList<Post> postsList)
	{
		var feedItems = new List<SyndicationItem>();

		foreach (var post in postsList)
		{
			var postUrl = _urlGenerator(post);
			var item = new SyndicationItem(
				post.Title, 
				new TextSyndicationContent(post.Description, TextSyndicationContentKind.Html),
				new Uri(postUrl), 
				post.UrlSlug,
				post.ModifiedDate ?? post.PostedDate);

			item.PublishDate = post.PostedDate;
			item.Summary = new TextSyndicationContent(post.ShortDescription, TextSyndicationContentKind.Html);

			//item.AddPermalink(new Uri(postUrl));

			item.Categories.Add(new SyndicationCategory(post.Category.Name));
			item.ElementExtensions.Add("meta", "", post.Meta);
			item.ElementExtensions.Add("tags", "", string.Join(", ", post.Tags.Select(x => x.Name)));

			if (!string.IsNullOrWhiteSpace(post.ImageUrl))
			{
				item.ElementExtensions.Add("image", "", $"{_rssSettings.Domain}/{post.ImageUrl}");
			}

			feedItems.Add(item);
		}

		return feedItems;
	}

	private async Task<IList<Post>> GetPostsAsync(CancellationToken cancellationToken)
	{
		var postQuery = new PostQuery();
		return await _blogRepository.GetPostsAsync(
			postQuery, 1, _rssSettings.MaximumItems, cancellationToken);
	}
}