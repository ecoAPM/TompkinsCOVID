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

        public Runner(ITwitter twitter, IHealthDepartment healthDept, Action<string> log)
        {
            _twitter = twitter;
            _healthDept = healthDept;
            _log = log;
        }
        
        public async Task Run()
        {
            _log("");
            var latest = await _twitter.GetLatestPostedDate();
            _log($"Last posted: {latest?.ToShortDateString() ?? "[never]"}");

            _log("");
            var records = await _healthDept.GetLatestRecords();
            _log($"{records.Count} records found, through {records.LastOrDefault()?.Date.ToShortDateString()}");

            var toTweet = records.Where(r => latest == null || r.Date > latest);
            foreach (var record in toTweet)
            {
                _log($"\nTweeting:\n{record}\n");
                await _twitter.Tweet(record);
                await Task.Delay(TimeSpan.FromSeconds(5));
            }

            _log("");
            _log("Done!");
        }
    }
}