using CulinaryBlog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CulinaryBlog.Infrastructure.Persistence.Configurations;

public class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
{
	public void Configure(EntityTypeBuilder<Ingredient> builder)
	{
		builder.HasKey(x => x.Id);

		builder.ToTable("Ingredients");

		builder.Property(x => x.Name)
			.IsRequired()
			.HasMaxLength(200);

		builder.Property(x => x.Quantity)
			.IsRequired()
			.HasMaxLength(50);

		builder.Property(x => x.Unit)
			.HasMaxLength(50);
	}
}