using System.Text;
using TatBlog.CopyChecker.Extensions;
using TatBlog.CopyChecker.Models;

namespace TatBlog.CopyChecker.Workers;

public class ProjectExporter
{
	public async Task ExportProjectsAsync(string targetFolder, IEnumerable<Project> projects)
	{
		var filePath = $"{targetFolder}\\projects-{DateTime.Now:yyyyMMdd-HHmmss}.csv";
		var writer = new StreamWriter(File.Create(filePath), Encoding.UTF8);

		var columns = new []
		{
			"StudentID",
			"StudentName",
			"SolutionId",
			"ProjectId",
			"ProjectName"
		};
		await writer.WriteLineAsync(columns.QuoteAndJoin());

		foreach (var project in projects)
		{
			await writer.WriteLineAsync(project.ToString());
		}

		writer.Close();
	}

	public async Task<int> ExportDuplicateAsync(string targetFolder, IEnumerable<Project> projects)
	{
		var filePath = $"{targetFolder}\\duplicates-{DateTime.Now:yyyyMMdd-HHmmss}.csv";
		var writer = new StreamWriter(File.Create(filePath), Encoding.UTF8);

		var columns = new[]
		{
			"ProjectId",
			"ProjectName",
			"Students"
		};
		await writer.WriteLineAsync(columns.QuoteAndJoin());

		var duplicates = projects
			.GroupBy(x => new {x.ProjectId, x.ProjectName})
			.Where(g => g.Count() > 1)
			.ToList();

		foreach (var projectGroup in duplicates)
		{
			var values = new[]
			{
				projectGroup.Key.ProjectId,
				projectGroup.Key.ProjectName,
				projectGroup.Select(x => $"{x.StudentCode} ({x.StudentName})").Join("; ")
			};
			await writer.WriteLineAsync(values.QuoteAndJoin());
		}

		writer.Close();

		return duplicates.Count;
	}
}