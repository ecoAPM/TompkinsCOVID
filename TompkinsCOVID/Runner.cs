using System;
using System.Linq;
using System.Threading.Tasks;

namespace TompkinsCOVID
{
    public class Runner
    {
        private readonly ITwitter _twitter;
        private readonly IHealthDepartment _healthDept;
        private readonly Action<string> _log;
        private readonly TimeSpan _wait;

        public Runner(ITwitter twitter, IHealthDepartment healthDept, Action<string> log, int wait = 5)
        {
            _twitter = twitter;
            _healthDept = healthDept;
            _log = log;
            _wait = TimeSpan.FromSeconds(wait);
        }

        private const string url = "https://docs.google.com/spreadsheets/u/2/d/e/2PACX-1vQvvugFsb4GePXQnmEZbgrtqmJRiaA7tO1UGSBwvBdhbJEmf2ntzE0am-x-Lo6mLPj9ASLpAg6UZsCF/pubhtml?gid=1214476126&single=true";
        
        public async Task Run()
        {
            _log("");
            var latest = await _twitter.GetLatestPostedDate();
            _log($"Last posted: {latest?.ToShortDateString() ?? "[never]"}");

            _log("");
            var records = await _healthDept.GetLatestRecords(url);
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
}