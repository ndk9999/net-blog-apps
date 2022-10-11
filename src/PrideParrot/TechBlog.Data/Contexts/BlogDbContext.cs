using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TechBlog.Core.Entities;

namespace TechBlog.Data.Contexts;

public class BlogDbContext : DbContext
{
	public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(BlogDbContext).Assembly);

		//var dataProtector = this.GetService<IPersonalDataProtector>();
		//var valueConverter = new PersonalDataConverter(dataProtector);

		//ApplyDataConverterFor<Account>(modelBuilder, valueConverter);
		//ApplyDataConverterFor<UserToken>(modelBuilder, valueConverter);
	}

	private void ApplyDataConverterFor<TEntity>(ModelBuilder modelBuilder, PersonalDataConverter converter) where TEntity : class
	{
		modelBuilder.Entity<TEntity>(b =>
		{
			var personalDataProps = typeof(TEntity).GetProperties()
				.Where(x => Attribute.IsDefined(x, typeof(ProtectedPersonalDataAttribute)));

			foreach (var prop in personalDataProps)
			{
				if (prop.PropertyType == typeof(string))
				{
					b.Property(typeof(string), prop.Name).HasConversion(converter);
				}
			}
		});
	}

	private class PersonalDataConverter : ValueConverter<string, string>
	{
		public PersonalDataConverter(IPersonalDataProtector protector)
			: base(s => protector.Protect(s), s => protector.Unprotect(s), default)
		{
		}
	}
}