using Microsoft.AspNetCore.Identity;
using TechBlog.Core.Entities;
using TechBlog.Data.Contexts;

namespace TechBlog.Data.Seeders;

public class DataSeeder : IDataSeeder
{
	private readonly IPasswordHasher<Account> _passwordHasher;
	private readonly BlogDbContext _dbContext;

	public DataSeeder(
		IPasswordHasher<Account> passwordHasher, 
		BlogDbContext dbContext)
	{
		_passwordHasher = passwordHasher;
		_dbContext = dbContext;
	}

	public void Initialize()
	{
		_dbContext.Database.EnsureCreated();

		var admin = AddRolesAndAccount();

		if (_dbContext.Set<Post>().Any())
		{
			return;
		}

		var tags = AddTags();
		var categories = AddCategories();
		var posts = AddPosts(admin, categories, tags);
	}

	private Account AddRolesAndAccount()
	{
		var roleNames = new[] { "Administrator", "Editor" };
		var roles = new List<Role>();

		foreach (var name in roleNames)
		{
			var role = _dbContext.Set<Role>().FirstOrDefault(x => x.Name == name);

			if (role is null)
			{
				role = new Role()
				{
					Name = name,
					NormalizedName = name.ToUpper(),
					ConcurrencyStamp = Guid.NewGuid().ToString("D")
				};
				_dbContext.Set<Role>().Add(role);
			}

			roles.Add(role);
		}

		var admin = new Account()
		{
			FullName = "Phuc Nguyen",
			UserName = "phucnv",
			NormalizedUserName = "PHUCNV",
			Email = "phuc.nguyen@devsoft.vn",
			NormalizedEmail = "PHUC.NGUYEN@DEVSOFT.VN",
			PhoneNumber = "0987654321",
			EmailConfirmed = true,
			PhoneNumberConfirmed = true,
			SecurityStamp = Guid.NewGuid().ToString("D")
		};

		if (!_dbContext.Set<Account>().Any(x => x.UserName == admin.UserName))
		{
			admin.PasswordHash = _passwordHasher.HashPassword(admin, "123456");

			_dbContext.Set<Account>().Add(admin);
			_dbContext.Set<UserInRole>().AddRange(roles.Select(x => new UserInRole()
			{
				Account = admin,
				Role = x
			}));
		}

		_dbContext.SaveChanges();

		return admin;
	}

	private IList<Tag> AddTags()
	{
		var tags = new List<Tag>()
		{
			new() {Name = "Google", Description = "Google applications and utilities", UrlSlug = "google"},
			new() {Name = "ASP.NET MVC", Description = "ASP.NET MVC", UrlSlug = "asp-net-mvc"},
			new() {Name = "Razor Page", Description = "Razor Page", UrlSlug = "razor-page"},
			new() {Name = "Blazor", Description = "Blazor", UrlSlug = "blazor"},
			new() {Name = "Deep Learning", Description = "Deep Learning", UrlSlug = "deep-learning"},
			new() {Name = "Neural Network", Description = "Neural Network", UrlSlug = "neural-network"},
			new() {Name = "Bootstrap", Description = "Bootstrap", UrlSlug = "bootstrap"},
			new() {Name = "Tailwind CSS", Description = "Tailwind CSS", UrlSlug = "tailwind-css"},
			new() {Name = "MongoDB", Description = "MongoDB", UrlSlug = "mongodb"},
			new() {Name = "Data Structures", Description = "Data Structures", UrlSlug = "data-structures"}
		};

		_dbContext.AddRange(tags);
		_dbContext.SaveChanges();

		return tags;
	}

	private IList<Category> AddCategories()
	{
		var categories = new List<Category>()
		{
			new() {Name = ".NET Core", Description = ".NET Core", UrlSlug = "net-core"},
			new() {Name = "Architecture", Description = "Architecture", UrlSlug = "architecture"},
			new() {Name = "Domain Driven Design", Description = "Domain Driven Design", UrlSlug = "domain-driven-design"},
			new() {Name = "Messaging", Description = "Messaging", UrlSlug = "messaging"},
			new() {Name = "OOP", Description = "Object-Oriented Programming", UrlSlug = "oop"},
			new() {Name = "Design Patterns", Description = "Design Patterns", UrlSlug = "design-patterns"},
			new() {Name = "Programming Languages", Description = "Programming Languages", UrlSlug = "programming-languages"},
			new() {Name = "Azure", Description = "Azure", UrlSlug = "azure"},
			new() {Name = "Practices", Description = "Practices", UrlSlug = "practices"},
			new() {Name = "Front-end Frameworks", Description = "Front-end Frameworks", UrlSlug = "front-end-frameworks"}
		};

		_dbContext.AddRange(categories);
		_dbContext.SaveChanges();

		return categories;
	}

	private IList<Post> AddPosts(Account admin, IList<Category> categories, IList<Tag> tags)
	{
		var posts = new List<Post>()
		{
			new()
			{
				Title = "ASP.NET Core Diagnostic Scenarios",
				ShortDescription = "David and friends has a great repository filled with examples of \"broken patterns\" in ASP.NET Core applications. It's a fantastic learning resource with both markdown and code that covers a number of common areas when writing scalable services in ASP.NET Core. Some of the guidance is general purpose but is explained through the lens of writing web services.",
				Description = "Here's a few great DON'T and DO examples, but be sure to Star the repo and check it out for yourself! This is somewhat advanced stuff but if you are doing high output low latency web services AT SCALE these tips will make a huge difference when you're doing a something a hundred thousand time a second! Here's a list of ASP.NET Core Guidance. This one is fascinating. ASP.NET Core doesn't buffer responses which allows it to be VERY scalable. Massively so. As such you do need to be aware that things need to happen in a certain order - Headers come before Body, etc so you want to avoid adding headers after the HttpResponse has started.",
				Meta = "David and friends has a great repository filled with examples of &#x27;broken ...",
				UrlSlug = "aspnet-core-diagnostic-scenarios",
				Published = true,
				PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
				ModifiedDate = null,
				Category = categories[0],
				Tags = new List<Tag>()
				{
					tags[0]
				}
			},
			new()
			{
				Title = "6 Productivity Shortcuts on Windows 10 & 11",
				ShortDescription = "I don’t know about you, but I’m obsessed with shortcuts. I’m much more productive when using just the keyboard, and having to use the mouse annoys me deeply. Over the years, I’ve learned many useful shortcuts that increase productivity. Many of them are for IDEs or other apps, but some of the best shortcuts are part of the operating system itself. Today we’ll cover 6 amazing shortcuts in Windows 10 and 11 that transformed the way I work and can make you much more productive. Some of them are more known than others, but all are brilliant.",
				Description = "You might know of the Ctrl + [number] shortcut in browsers that navigate to a tab in the pressed number’s place. e.g Ctrl + 2 will move to the 2nd tab from the left, Ctrl + 3 to the third, and so on. This is cool, but did you know Windows also has the same feature for apps in the Taskbar? Pressing Windows key + 1 opens the first app on the left, Windows key + 2 opens the second one, and so on up to Windows key + 0.\r\n\r\nNow if you pin your most used apps to the taskbar, this becomes really useful. For example, Windows key + 1 always opens my browser, Windows key + 2 always opens Free Commander, followed by Outlook, etc. I got 7 of my most used apps pinned in the same places on all my computers, which means I’m saving a couple of seconds a hundred times a day and more importantly, avoid reaching for the mouse.",
				Meta = "6 Productivity Shortcuts on Windows 10 & 11 I wish I knew as a Junior Software Developer",
				UrlSlug = "windows-productivity-shortcuts",
				Published = true,
				PostedDate = new DateTime(2022, 9, 25, 16, 20, 0),
				ModifiedDate = null,
				Category = categories[4],
				Tags = new List<Tag>()
				{
					tags[2], tags[7]
				}
			},
			new()
			{
				Title = "Azure Virtual Machines vs App Services",
				ShortDescription = "Azure Virtual Machines and App Services are the two basic pillars of Azure cloud services. Both offerings provide a way for you to execute workloads or host your server in the cloud. In both, you pay for some virtual machine in an Azure data center that runs your code. But that’s where the similarities end. One is bare bones infrastructure, whereas the other is a managed platform. One is customizable but hard to manage, whereas the other requires forces you to use the specific tooling and configuration Azure offers.",
				Description = "VMs are easy to understand. You rent a virtual machine from Azure and do whatever you want with it. You can connect remotely to that machine, add or delete files, install programs, expose ports, run servers, etc. It’s somewhat similar to the old on-premise concept, but there are plenty of advantages to using cloud: you don’t need to handle the hardware purchase, the physical storage, and the maintenance of the computer. Azure takes care of broadband connectivity and network uptime. Most importantly, you can scale up and down as you please, though with Azure Virtual Machine you’ll need to manage it yourself with VM scale sets.\r\n\r\nAzure takes care of a lot for you, but as opposed to an App Service, you still have to manage and run the server yourself. You’ll have to install a server tech (like IIS), a runtime (like .NET), and a deploy mechanism (like web deploy). You’ll have to take care of OS updates and security patches. And you’ll have to install and manage whatever monitoring and debugging tools you need. If there’s a crash, a hang, or a performance problem, you need to develop a way to investigate it yourself.",
				Meta = "Azure Virtual Machines vs App Services",
				UrlSlug = "azure-virtual-machines-vs-app-services",
				Published = true,
				PostedDate = new DateTime(2022, 7, 19, 6, 31, 0),
				ModifiedDate = null,
				Category = categories[8],
				Tags = new List<Tag>()
				{
					tags[3], tags[5]
				}
			},
			new()
			{
				Title = "Productivity Boost After Porting from WordPress to Static Site Generation",
				ShortDescription = "After 6 years of hosting my blog in WordPress, I ported it to Hugo, a static site generator. I used to be a big WordPress believer. I’d tell anyone who wished to hear, and many who didn’t, that WP was the answer to everything. Whether you’re building a personal blog, an e-commerce site, or a portfolio showcase. That belief was crumbling for the last few years up to the point I turned almost 180 degrees. WordPress is very much not the answer to everything, and you should be very mindful when using it.",
				Description = "WordPress is particularly interesting considering that about 40% of all websites run on it. At its core, WP is a content management system (CMS). It allows you to add content like blog posts, reviews, images, etc. A backend admin dashboard allows you to add and edit that content, design the theme, manage users, etc.\r\n\r\nOn the site layout side of things, WordPress is a massive beast with an endless amount of themes and plugins to choose from. There is an entire industry that creates and sells whatever your heart desires, whether it’s an e-commerce plugin, a photo studio theme, a paid subscription site, an SEO plugin, etc. No other web platform comes close to the ecosystem of WordPress.\r\n\r\nIf we look under the hood, WP includes a PHP server with a MySQL database that stores your content. When a user enters a page on the site, the request goes to the PHP server, which queries the database for the content and runs PHP code that generates the HTML, CSS, and JavaScript that you’ll eventually see on the client side.",
				Meta = "Productivity Boost After Porting from WordPress to Static Site Generation",
				UrlSlug = "porting-wordpress-to-ssg",
				Published = true,
				PostedDate = new DateTime(2022, 4, 11, 8, 57, 0),
				ModifiedDate = null,
				Category = categories[3],
				Tags = new List<Tag>()
				{
					tags[2], tags[4], tags[8]
				}
			},
			new()
			{
				Title = "Beware of records, with expressions and calculated properties",
				ShortDescription = "I’ve been using C# records a lot since the feature was introduced. However, when using them, we really need to understand how they work, otherwise we might face unexpected surprises.",
				Description = "Given their terseness, even ignoring the other records characteristics, as soon as I’m creating a type that I’m expecting to be immutable, I immediately start with a record. However this might not always be adequate, and in this post we’ll look at a very specific example where I introduced a bug in the code due to not taking into consideration all of the records characteristics. This solves the problem, and might be a valid solution in general. It has one potential issue though: every time we use SomeCalculatedValue, like the method that the property get accessor is, the value will be calculated. Depending on the way the property is used, it might not be a problem, or it might be, as we’re always repeating the same logic and creating new objects to return. In my case, I was using the property multiple times, and the calculation logic was a bit more complex than this simplified example, so it would be nice to avoid executing this code more often than actually needed.",
				Meta = "Beware of records, with expressions and calculated properties",
				UrlSlug = "beware-of-records-with-expressions-and-calculated-properties",
				Published = true,
				PostedDate = new DateTime(2022, 10, 1, 14, 10, 0),
				ModifiedDate = null,
				Category = categories[2],
				Tags = new List<Tag>()
				{
					tags[3], tags[9]
				}
			},
			new()
			{
				Title = "Outbox meets change data capture",
				ShortDescription = "Another video on using change data capture (often referred to as CDC), to hook up into the database transaction log, forwarding incoming entries to the outbox table.",
				Description = "Another video on using change data capture (often referred to as CDC), to hook up into the database transaction log, forwarding incoming entries to the outbox table.\r\n\r\nPreviously, we used Debezium for this, but I was left wondering, what if I want to implement something similar with .NET? Turns out it’s not super hard, with the help of Npgsql, which provides us with APIs to hook into PostgreSQL Write Ahead Log, so we can read the incoming outbox messages and forward them to Kafka.",
				Meta = "Outbox meets change data capture - hooking into the Write-Ahead Log",
				UrlSlug = "outbox-meets-change-data-capture-hooking-into-the-write-ahead-log",
				Published = true,
				PostedDate = new DateTime(2022, 8, 19, 20, 6, 0),
				ModifiedDate = null,
				Category = categories[9],
				Tags = new List<Tag>()
				{
					tags[1], tags[6], tags[5]
				}
			},
			new()
			{
				Title = "Array or object JSON deserialization",
				ShortDescription = "Ah, the joys of integrating with third-party APIs… We always end up having to hammer something to get things working. This is a post about one of such situations, resorting to some JSON deserialization trickery (via JsonConverter) to be able to get things working.",
				Description = "This is a post about one of such situations, resorting to some JSON deserialization trickery (via JsonConverter) to be able to get things working. So, I’m integrating with this API, which in a specific endpoint, for some reason, returns an object in which one of its properties, when empty, is an empty array, but when there’s data, it’s an object with an items property which in turn is the array.",
				Meta = "Array or object JSON deserialization (feat. .NET & System.Text.Json)",
				UrlSlug = "array-or-object-deserialization",
				Published = true,
				PostedDate = new DateTime(2022, 5, 11, 9, 27, 0),
				ModifiedDate = null,
				Category = categories[6],
				Tags = new List<Tag>()
				{
					tags[4]
				}
			},
			new()
			{
				Title = "OpenAPI extensions and Swashbuckle",
				ShortDescription = "Let’s take a quick look at OpenAPI extensions (which I discovered were a thing last week) and how to add them to our API description using ASP.NET Core and Swashbuckle.",
				Description = "OpenAPI extensions are what you’re maybe already inferring from the name, a way to extend an API description, with custom properties, to be able to describe things that aren’t supported by the OpenAPI specification.\r\n\r\nThese custom properties names need to be prefixed with x- (e.g. x-your-property), but only at the root level, i.e. if this property is an object, the object’s properties don’t need the prefix. Of course, this isn’t a great to do things, so I started looking into ways to add custom stuff using Swashbuckle. Mind you, given I didn’t know about OpenAPI extensions, also didn’t know the right terms to use in the search, but at some point, after scouring a bunch of blog posts and Stack Overflow entries, found out what was the name of what I needed, so from then on, things got easier.",
				Meta = "OpenAPI extensions and Swashbuckle",
				UrlSlug = "openapi-extensions-and-swashbuckle",
				Published = true,
				PostedDate = new DateTime(2022, 2, 19, 10, 20, 0),
				ModifiedDate = null,
				Category = categories[5],
				Tags = new List<Tag>()
				{
					tags[2], tags[7]
				}
			},
			new()
			{
				Title = "Dart: Formatting and Trailing Commas",
				ShortDescription = "Trailing commas may sound like a minor aspect of the Dart language, but they have a major impact on the formatting of your code. This article explains deterministic formatting, how trailing commas affect it, why you should use them, and how to add the dart_code_metrics package to enforce them for better formatting.",
				Description = "A code formatter or code beautifier is a tool that formats your code. Dart has a tool called dart format. Formatters take raw code (and sometimes configuration) as input and output formatted code. \r\n\r\nGiven some raw semantically identical code, a deterministic formatter always outputs identical text. In other words, if you copy, paste a chunk of code, and put extra whitespaces in the second copy, applying the deterministic formatter will always result in the exact same text. No matter how you format the original code with whitespaces, the formatter will always produce a predictable code result. ",
				Meta = "Dart: Formatting and Trailing Commas",
				UrlSlug = "dart-formatting-and-trailing-commas",
				Published = true,
				PostedDate = new DateTime(2022, 7, 30, 19, 41, 0),
				ModifiedDate = null,
				Category = categories[0],
				Tags = new List<Tag>()
				{
					tags[5], tags[0], tags[9]
				}
			},
			new()
			{
				Title = "Device.Net Project Status",
				ShortDescription = "Device.Net is a cross-platform framework that attempts to put a layer over the top of USB and Hid. It runs on Android, UWP, .NET Framework and .NET. I wrote this for a crypto project (hardwarewallets) a long time ago, and this has ballooned. I'm pausing indefinitely and looking for other people to contribute. This post also touches on funding open source projects.",
				Description = "In 2022 custom USB and Hid are still a reality. We all have USB devices and apps that communicate with them. I originally created thelibraries Trezor.Net, KeepKey.Net and Ledger.Net to communicate with cryptocurrency hardwarewallets. You can use the same library to communicate with these devices on a PC or Android phone. Many device manufacturers contacted me and asked for help with .NET apps. This will continue because there is simply no alternative to USB yet. Bluetooth is not an alternative for low latency requirements like gaming peripherals.\r\n\r\nMy original intention was to create something that the .NET community could get behind and contribute to. Sadly, the main contributions have only been to fix their projects' issues. In most cases, these were not open source projects and did not benefit the .NET community. In multiple cases, people took my open source code, close sourced it, and profited from my code. One person even had the nerve to ask for help with their closed source project after essentially profiting from my work.",
				Meta = "Device.Net Project Status",
				UrlSlug = "device-net-project-status",
				Published = true,
				PostedDate = new DateTime(2022, 8, 16, 17, 30, 0),
				ModifiedDate = null,
				Category = categories[1],
				Tags = new List<Tag>()
				{
					tags[4], tags[5]
				}
			},
			new()
			{
				Title = "Google Cloud Run",
				ShortDescription = "Google Cloud Run is a serverless container app service. You can deploy containerised apps to the cloud, which will autoscale horizontally with minimal configuration. It is an alternative to Kubernetes, but you only pay for usage. You do not pay for server uptime when there is no server usage. ",
				Description = "Containers give your app portability. If your app runs on a Linux Docker container, your app will run just about anywhere in the cloud, on-prem or on a well-speced computer. Containers decouple your app from the Cloud Host. For example, if I build a .NET Web API, it will require the .NET runtime and perhaps other dependencies. I can deploy the app to a cloud host that supports .NET, but what if they change one of the .NET dependencies? It might mean that my app becomes incompatible with their version of .NET. My app needs to be compatible with their service and not the other way around.\r\n\r\nContainerization solves this issue. I can install any version of .NET on the container and deploy it. The cloud host doesn't need to know anything about the version of .NET I am using. I can configure the container however I like. Infact, I can install any version of any runtime that suits my needs on the container. ",
				Meta = "Google Cloud Run",
				UrlSlug = "google-cloud-run",
				Published = true,
				PostedDate = new DateTime(2022, 1, 14, 8, 25, 0),
				ModifiedDate = null,
				Category = categories[4],
				Tags = new List<Tag>()
				{
					tags[1], tags[8]
				}
			},
			new()
			{
				Title = "Papilio: An Intro",
				ShortDescription = "Flutter gives you a powerful toolset for building rich cross-platform apps. You can build single-source apps on macOS, Windows or Linux and run those apps on phones, desktops, and in the browser. Dart is an elegant, modern language that lets you build fast, responsive, and maintainable apps. It's also familiar to Java and C# developers.",
				Description = "Bloc is a state management pattern that allows you to decouple your business logic from your UI. You don't need to know anything about BloC to use Papilio. You define BloC event types and send those events to the BloC, which handles them and emits state changes on a stream. Your UI will update automatically. Under the hood, this uses a flutter StreamBuilder that listens to state changes so you can write StatelessWidgets and never need to call setState. Papilio implements the RouterDelegate and  RouteInformationParser for Material app routing. You use the MaterialApp.router constructor to create a MaterialApp that uses the Router instead of a Navigator. \r\nThis widget listens for routing information from the operating system (e.g. an initial route provided on app startup, a new route obtained when an intent is received, or a notification that the user hit the system back button), parses route information into data of type T, and then converts that data into Page objects that it passes to a Navigator.",
				Meta = "Introduction to Papilio",
				UrlSlug = "intruduction-to-papilio",
				Published = true,
				PostedDate = new DateTime(2022, 6, 11, 13, 55, 0),
				ModifiedDate = null,
				Category = categories[7],
				Tags = new List<Tag>()
				{
					tags[9], tags[3], tags[0]
				}
			},
			new()
			{
				Title = "Performance Vs. Scalability",
				ShortDescription = "Performance and scalability are two related but separate aspects of a system. However, there is a lot of confusion around the two terms, which often leads to architectural mistakes. This article talks about the difference between the two concepts and how to improve them.",
				Description = "Performance is about how fast your system works. It's the experience users have when waiting for your system to do something, and it's how fast your underlying code runs. Network latency, bandwidth, and other factors also affect performance. \r\n\r\nWhen the system does not have a lot of data or users, performance is usually acceptable and easy to manage. When the amount of data and number of concurrent users grow, the servers' resources may reach their maximum, and this will cause performance degradation.\r\n\r\nHowever, there is a key point here. The system's design and the code quality have a huge impact on how quickly the server's resources reach their maximum capacity. You can pay for an extremely expensive database server, but data retrieval will consume a lot of resources if you do not put indexes in the right place. Conversely, efficient code can avoid resource depletion even on cheap servers.\r\n\r\nIf you've done elementary computer science and learned the basics of data structures and algorithms, you will be aware of how quickly a poor implementation can chew memory or CPU power. This is what we mean by resources. Poor code will chew these up, your system will reach capacity, and users will experience poor performance.",
				Meta = "Performance Vs. Scalability",
				UrlSlug = "performance-vs-scalability",
				Published = true,
				PostedDate = new DateTime(2022, 1, 1, 10, 20, 0),
				ModifiedDate = null,
				Category = categories[4],
				Tags = new List<Tag>()
				{
					tags[1], tags[5]
				}
			},
			new()
			{
				Title = "C# Code Rules",
				ShortDescription = "The C# Compiler's name is Roslyn. Roslyn has a very large set of analyzers to check the quality of your code, but you must turn these analyzers on before they start doing anything. This post gives you some quick information on why it's important to turn these analyzers on in your C# projects, how to do that, and how to configure them.",
				Description = "Code rules are rules that the compiler or code analysis tool enforces at the build level. They enforce Coding Conventions, detect mistakes, and provide the tooling to do bulk fixes. Roslyn and other .NET/C# tools use Static Code Analysis on your codebase to check where there are violations in your code. Rider and Resharper provide static code analysis tools on top of the existing Roslyn code analysis. I recommend supplementing your code analysis with Rider or Resharper, but this article focuses on the core Roslyn analyzers. Check out the overview here. This is a controversial topic, and I'm not going to fill this article with arguments for using code rules. My years of experience tell me that code rules are a very good thing and that teams that use them have fewer issues.",
				Meta = "C# Code Rules",
				UrlSlug = "code-rules",
				Published = true,
				PostedDate = new DateTime(2022, 4, 24, 9, 21, 0),
				ModifiedDate = null,
				Category = categories[8],
				Tags = new List<Tag>()
				{
					tags[6]
				}
			},
			new()
			{
				Title = "JWT creation and validation in Python using Authlib",
				ShortDescription = "Authlib is a Python library that provides various OAuth, OpenID Connect, and JWT functionality. Authlib is my preferred library for JWT functionality, as it is one of the better Python implementations for JWT best practices, designed with OAuth and OpenID Connect in mind.",
				Description = "To create a JWT, you will need a private key to sign it with. This should use an asymmetric signing algorithm, such as RS256, rather than something symmetric like HS256.\r\n\r\nIn this example, I’ll load in a signing key from a JSON Web Key (JWK) that contains a key suitable for ES256. Hopefully, it’s obvious that this key is no longer private and only suitable for demos. If you want to quickly generate your own key for testing & documentation, check out the JWK generation feature of my JWT tool.",
				Meta = "JWT creation and validation in Python using Authlib",
				UrlSlug = "authlib-python-jwt",
				Published = true,
				PostedDate = new DateTime(2022, 8, 25, 18, 44, 0),
				ModifiedDate = null,
				Category = categories[2],
				Tags = new List<Tag>()
				{
					tags[6], tags[7]
				}
			},
		};

		_dbContext.AddRange(posts);
		_dbContext.SaveChanges();

		return posts;
	}
}