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
            
            var latest = await twitter.GetLatestPostedDate();
            var records = await healthDept.GetLatestRecords();
            var next = records.FirstOrDefault(r => latest == null || r.Date > latest);

            if (next != null)
            {
                var content =  twitter.CreateTweetContent(next);
                await twitter.Tweet(content);
            }
        }
    }
}
