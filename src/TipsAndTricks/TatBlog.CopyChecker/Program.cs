using System.Diagnostics;
using TatBlog.CopyChecker.Workers;

var stopwatch = new Stopwatch();
stopwatch.Start();

Console.WriteLine("Start checking ...");

var folderPath = args.Any() ? args[0] : "C:\\Users\\Phuc Nguyen\\Downloads\\Firefox\\PTUDWNC-Lab03-Practice";

var parser = new ProjectParser();
var projects = await parser.ParseAsync(folderPath);

var exporter = new ProjectExporter();
await exporter.ExportProjectsAsync(folderPath, projects);

var duplicateCount = await exporter.ExportDuplicateAsync(folderPath, projects);

stopwatch.Stop();
Console.WriteLine("Finish checking ...");

Console.WriteLine("Found {0} duplicates in {1} milliseconds",
	duplicateCount, stopwatch.ElapsedMilliseconds);

Console.WriteLine("".PadRight(80, '='));

foreach (var project in projects)
{
	Console.WriteLine(project);
}