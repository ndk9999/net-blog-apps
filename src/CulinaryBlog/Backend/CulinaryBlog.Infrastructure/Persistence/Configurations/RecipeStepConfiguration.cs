using CulinaryBlog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CulinaryBlog.Infrastructure.Persistence.Configurations;

public class RecipeStepConfiguration : IEntityTypeConfiguration<RecipeStep>
{
	public void Configure(EntityTypeBuilder<RecipeStep> builder)
	{
		builder.HasKey(x => x.Id);

		builder.ToTable("RecipeSteps");

		builder.Property(x => x.Description)
			.IsRequired()
			.HasMaxLength(2000);

		builder.Property(x => x.Title)
			.HasMaxLength(100);

		builder.Property(x => x.ImageUrl)
			.HasMaxLength(2048);
	}
}