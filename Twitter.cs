using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TompkinsCOVID
{
    public class Twitter
    {
        private readonly HttpClient _http;

        public Twitter(HttpClient http)
            => _http = http;

        public async Task<DateTime?> GetLatestPostedDate()
        {
            return await Task.FromResult(DateTime.Parse("7/22/21"));
        }

        public string CreateTweetContent(Record record)
            => $@"
                {record.Date.ToShortDateString()}
                
                {record.PositiveToday:N0} new positive cases
                {record.ActiveCases:N0} active cases
                {record.PositiveTotal:N0} total positive cases

                {record.Hospitalized:N0} currently hospitalized
                {record.Deceased:N0} deceased
                {record.Recovered:N0} recovered

                {record.TestedToday:N0} tested today
                {record.TestedTotal:N0} tested total

                {record.PartiallyVaccinated:N0} partially vaccinated
                {record.FullyVaccinated:N0} fully vaccinated
            ";

        public async Task Tweet(string content)
        {
            await Task.Delay(TimeSpan.Zero);
            Console.WriteLine(content);
        }
    }
}