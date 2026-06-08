using TatBlog.CopyChecker.Extensions;

namespace TatBlog.CopyChecker.Models;

public class Project
{
	public string StudentCode { get; set; }

	public string StudentName { get; set; }

	public string ProjectId { get; set; }

	public string SolutionId { get; set; }

	public string ProjectName { get; set; }

	public override string ToString()
	{
		return string.Join(",", new[]
		{
			StudentCode.Quote(),
			StudentName.Quote(),
			SolutionId.Quote(),
			ProjectId.Quote(),
			ProjectName.Quote()
		});
	}
}