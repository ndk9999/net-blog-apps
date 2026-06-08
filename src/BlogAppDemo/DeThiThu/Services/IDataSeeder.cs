namespace DeThiThu.Services;

public interface IDataSeeder
{
	Task ImportAsync(CancellationToken cancellationToken = default);
}