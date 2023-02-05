using Microsoft.Extensions.Configuration;

namespace TompkinsCOVID;

public sealed class App
{
	private readonly ISocialMediaManager _socialMediaManager;
	private readonly IHealthDepartment _healthDept;
	private readonly Action<string> _log;
	private readonly string _username;
	private readonly TimeSpan _wait;

	public App(ISocialMediaManager socialMediaManager, IHealthDepartment healthDept, Action<string> log, IConfiguration config)
	{
		_socialMediaManager = socialMediaManager;
		_healthDept = healthDept;
		_log = log;
		_username = config["username"] ?? string.Empty;
		_wait = TimeSpan.FromSeconds(double.Parse(config["wait"] ?? string.Empty));
	}

	public async Task Run(string? arg = null)
	{
		_log("");
		var latest = DateOnly.TryParse(arg, out var argDate)
			? argDate
			: await _socialMediaManager.GetLatestPostedDate(_username);
		_log($"Last posted: {latest?.ToShortDateString() ?? "[never]"}");
		var recordDate = latest?.AddDays(-ActiveCaseCalculator.ActiveDays * 2)
			?? DateOnly.FromDateTime(DateTime.UnixEpoch);

		_log("");
		var records = await _healthDept.GetRecordsSince(recordDate);
		_log($"{records.Count} records found, for {records.LastOrDefault().Key.ToShortDateString()} through {records.FirstOrDefault().Key.ToShortDateString()}");

		var toPost = records
			.Where(r => ShouldPost(r.Value, latest))
			.OrderBy(r => r.Key)
			.ToArray();

		if (!toPost.Any())
		{
			_log("Nothing to post!");
		}

		foreach (var record in toPost)
		{
			record.Value.ActiveCases ??= records.CalculateActiveCases(record.Key);

			_log($"\nPosting:\n{record.Value}\n");
			await _socialMediaManager.Post(record.Value);
			await Task.Delay(_wait);
		}

		_log("");
		_log("Done!");
	}

	private static bool ShouldPost(Record r, DateOnly? latest)
		=> r.Date > latest && r.PositiveToday is not null;
}