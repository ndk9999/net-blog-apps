using System.Text.RegularExpressions;
using TatBlog.CopyChecker.Extensions;
using TatBlog.CopyChecker.Models;

namespace TatBlog.CopyChecker.Workers;

public class ProjectParser
{
	private readonly Regex _studentCodeRegex = new Regex("\\d{7}", RegexOptions.Compiled);
	private readonly Regex _solutionIdRegex = new Regex("SolutionGuid[\\s=]+[{](?<sid>[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12})[}]", RegexOptions.Compiled);
	private readonly Regex _projectIdRegex = new Regex("[{](?<sid>[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12})[}].+?\"(?<name>.+?)\".+?[{](?<pid>[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12})[}]", RegexOptions.Compiled);

	public async Task<List<Project>> ParseAsync(string folderPath)
	{
		var subFolders = Directory.EnumerateDirectories(folderPath);
		var projects = new List<Project>();

		foreach (var solutionFolder in subFolders)
		{
			var studentId = GetStudentCode(solutionFolder);

			if (studentId.IsEmpty())
			{
				Console.WriteLine("No Student Code: {0}", solutionFolder);
				continue;
			}

			var studentName = GetStudentName(solutionFolder);
			var solutionFile = GetSolutionFile(solutionFolder);

			if (solutionFile.IsEmpty())
			{
				Console.WriteLine("No Solution File: {0}", solutionFolder);
				continue;
			}

			var items = await ExtractProjectAsync(studentId, studentName, solutionFile);
			projects.AddRange(items);
		}

		return projects;
	}

	private string GetStudentCode(string folderPath)
	{
		var match = _studentCodeRegex.Match(folderPath);
		return match.Success ? match.Value : null;
	}

	private string GetStudentName(string folderPath)
	{
		var eIdx = folderPath.IndexOf("_assignsubmission_");
		var sIdx = eIdx;

		while (sIdx > 0 && folderPath[sIdx] != '\\') sIdx--;

		return folderPath.Substring(sIdx + 1, eIdx - sIdx - 1).RemoveCommas();
	}

	private string GetSolutionFile(string folderPath)
	{
		return Directory
			.EnumerateFiles(folderPath, "*.sln", SearchOption.AllDirectories)
			.FirstOrDefault();
	}

	private async Task<List<Project>> ExtractProjectAsync(
		string studentCode, string studentName, string solutionFile)
	{
		var projects = new List<Project>();
		var content = await File.ReadAllTextAsync(solutionFile);
		var solutionIdMatch = _solutionIdRegex.Match(content);
		
		var projectMatches = _projectIdRegex.Matches(content);

		foreach (Match match in projectMatches)
		{
			if (!match.Success) continue;

			projects.Add(new Project()
			{
				StudentCode = studentCode,
				StudentName = studentName,
				ProjectId = match.Groups["pid"].Value,
				SolutionId = solutionIdMatch.Groups["sid"].Value,
				ProjectName = match.Groups["name"].Value,
			});
		}

		return projects;
	}
}