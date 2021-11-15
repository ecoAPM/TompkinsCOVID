using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace TompkinsCOVID;

public class Runner
{
	private readonly ITwitter _twitter;
	private readonly IHealthDepartment _healthDept;
	private readonly Action<string> _log;
	private readonly string _url;
	private readonly TimeSpan _wait;

	public Runner(ITwitter twitter, IHealthDepartment healthDept, Action<string> log, IConfigurationRoot config)
	{
		_twitter = twitter;
		_healthDept = healthDept;
		_log = log;
		_url = config["url"];
		_wait = TimeSpan.FromSeconds(double.Parse(config["wait"]));
	}

	public async Task Run()
	{
		_log("");
		var latest = await _twitter.GetLatestPostedDate();
		_log($"Last posted: {latest?.ToShortDateString() ?? "[never]"}");

		_log("");
		var records = await _healthDept.GetLatestRecords(_url);
		_log($"{records.Count} records found, through {records.LastOrDefault()?.Date.ToShortDateString()}");

		var toTweet = records.Where(r => latest == null || r.Date > latest).ToList();
		foreach (var record in toTweet)
		{
			_log($"\nTweeting:\n{record}\n");
			await _twitter.Tweet(record);
			await Task.Delay(_wait);
		}

		_log("");
		_log("Done!");
	}
}
