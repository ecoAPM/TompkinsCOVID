using Microsoft.Extensions.Configuration;

namespace TompkinsCOVID;

public class Runner
{
	private readonly ITwitter _twitter;
	private readonly IHealthDepartment _healthDept;
	private readonly Action<string> _log;
	private readonly string _url;
	private readonly string _username;
	private readonly TimeSpan _wait;

	public Runner(ITwitter twitter, IHealthDepartment healthDept, Action<string> log, IConfiguration config)
	{
		_twitter = twitter;
		_healthDept = healthDept;
		_log = log;
		_url = config["url"] ?? string.Empty;
		_username = config["username"] ?? string.Empty;
		_wait = TimeSpan.FromSeconds(double.Parse(config["wait"] ?? string.Empty));
	}

	public async Task Run(string? arg = null)
	{
		_log("");
		var latest = DateTime.TryParse(arg, out var argDate)
			? argDate
			: await _twitter.GetLatestPostedDate(_username);
		_log($"Last posted: {latest?.ToShortDateString() ?? "[never]"}");

		_log("");
		var records = await _healthDept.GetRecords(_url);
		_log($"{records.Count} records found, through {records.LastOrDefault().Key.ToShortDateString()}");

		var toTweet = records.Where(r => latest == null || ShouldTweet(r.Value, latest.Value)).ToList();
		foreach (var record in toTweet)
		{
			record.Value.ActiveCases ??= records.CalculateActiveCases(record.Key);

			_log($"\nTweeting:\n{record.Value}\n");
			await _twitter.Tweet(record.Value);
			await Task.Delay(_wait);
		}

		_log("");
		_log("Done!");
	}

	private static bool ShouldTweet(Record r, DateTime latest)
		=> r.Date > latest && r.PositiveToday != null;
}