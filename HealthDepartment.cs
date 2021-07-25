using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp;

namespace COVID
{
    public class HealthDepartment
    {
        private readonly HttpClient _http;

        public HealthDepartment(HttpClient http)
            => _http = http;

        private const string url = "https://docs.google.com/spreadsheets/u/2/d/e/2PACX-1vQvvugFsb4GePXQnmEZbgrtqmJRiaA7tO1UGSBwvBdhbJEmf2ntzE0am-x-Lo6mLPj9ASLpAg6UZsCF/pubhtml?gid=1214476126&single=true";

        public async Task<IList<Record>> GetLatestRecords()
        {
            var html = await _http.GetAsync(url);
            var content = await html.Content.ReadAsStringAsync();

            var browser = BrowsingContext.New(Configuration.Default);
            var dom = await browser.OpenAsync(r => r.Content(content));

            var records = new List<Record>();
            var rows = dom.DocumentElement.QuerySelectorAll("tbody tr");
            foreach (var row in rows)
            {
                var cells = row.QuerySelectorAll("td");

                var day = cells[0].TextContent;
                if (string.IsNullOrWhiteSpace(day) || !DateTime.TryParse(day, out var date))
                    continue;

                records.Add(new Record(cells));
            }

            return records;
        }
    }
}