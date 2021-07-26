using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Tweetinvi;

namespace TompkinsCOVID
{
    public static class Program
    {
        public static async Task Main()
        {
            var consumerKey = Environment.GetEnvironmentVariable("ConsumerKey");
            var consumerSecret = Environment.GetEnvironmentVariable("ConsumerSecret");
            var accessKey = Environment.GetEnvironmentVariable("AccessKey");
            var accessSecret = Environment.GetEnvironmentVariable("AccessSecret");

            var twitter = new Twitter(new TwitterClient(consumerKey, consumerSecret, accessKey, accessSecret));
            var healthDept = new HealthDepartment(new HttpClient());
            
            Console.WriteLine();
            var latest = await twitter.GetLatestPostedDate();
            Console.WriteLine($"Last posted: {latest?.ToShortDateString() ?? "[never]"}");
            
            Console.WriteLine();
            var records = await healthDept.GetLatestRecords();
            Console.WriteLine($"{records.Count()} records found, through {records.LastOrDefault()?.Date.ToShortDateString()}");

            var toTweet = records.Where(r => latest == null || r.Date > latest);
            foreach(var record in toTweet)
            {
                Console.WriteLine($"\nTweeting:\n{record}\n");
                await twitter.Tweet(record);
                await Task.Delay(TimeSpan.FromSeconds(5));
            }

            Console.WriteLine();
            Console.WriteLine("Done!");
        }
    }
}
