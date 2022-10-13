﻿using TechBlog.Core.Contracts;

namespace TechBlog.Core.Entities;

public class Category : IEntity
{
	public int Id { get; set; }

	public string Name { get; set; }

	public string UrlSlug { get; set; }

	public string Description { get; set; }

	public bool ShowOnMenu { get; set; }


	public IList<Post> Posts { get; set; }
}