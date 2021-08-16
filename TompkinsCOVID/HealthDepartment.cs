using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp;

namespace TompkinsCOVID
{
    public class HealthDepartment : IHealthDepartment
    {
        private readonly HttpClient _http;

        public HealthDepartment(HttpClient http)
            => _http = http;

        private const string url = "https://docs.google.com/spreadsheets/u/2/d/e/2PACX-1vQvvugFsb4GePXQnmEZbgrtqmJRiaA7tO1UGSBwvBdhbJEmf2ntzE0am-x-Lo6mLPj9ASLpAg6UZsCF/pubhtml?gid=1214476126&single=true";

        public async Task<IList<Record>> GetLatestRecords()
        {
            var html = await _http.GetAsync(url);
            var content = html.Content.ReadAsStreamAsync();

            var browser = BrowsingContext.New(Configuration.Default);
            var dom = await browser.OpenAsync(async r => r.Content(await content));

            var rows = dom.DocumentElement.QuerySelectorAll("tbody tr");
            var records = new List<Record>();
            foreach (var row in rows)
            {
                var cells = row.QuerySelectorAll("td");

                var day = cells[0].TextContent;
                if (string.IsNullOrWhiteSpace(day) || !DateTime.TryParse(day, out _))
                    continue;

                records.Add(new Record(cells));
            }

            return records;
        }
    }
}