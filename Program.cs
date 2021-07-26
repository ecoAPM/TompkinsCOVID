using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TompkinsCOVID
{
    public static class Program
    {
        public static async Task Main()
        {
            var http = new HttpClient();
            var twitter = new Twitter(http);
            var healthDept = new HealthDepartment(http);
            
            Console.WriteLine();
            var latest = await twitter.GetLatestPostedDate();
            Console.WriteLine($"Last posted: {latest?.ToShortDateString() ?? "[never]"}");
            
            Console.WriteLine();
            var records = await healthDept.GetLatestRecords();
            Console.WriteLine($"{records.Count()} records found, through {records.LastOrDefault()?.Date.ToShortDateString()}");

            var next = records.FirstOrDefault(r => latest == null || r.Date > latest);
            if (next != null)
            {
                Console.WriteLine($"\nTweeting:\n{next}\n");
                await twitter.Tweet(next);
            }

            Console.WriteLine();
            Console.WriteLine("Done!");
        }
    }
}
