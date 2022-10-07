﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TechBlog.Core.Entities;

namespace TechBlog.Core.Mappings;

public class UserInRoleMap : IEntityTypeConfiguration<UserInRole>
{
	public void Configure(EntityTypeBuilder<UserInRole> builder)
	{
		builder.ToTable("UserInRoles");

		builder.HasKey(r => r.Id);
	}
}
