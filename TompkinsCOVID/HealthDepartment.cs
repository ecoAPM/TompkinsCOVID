using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Io;

namespace TompkinsCOVID;

public class HealthDepartment : IHealthDepartment
{
	private readonly HttpClient _http;

	public HealthDepartment(HttpClient http)
		=> _http = http;

	public async Task<IList<Record>> GetLatestRecords(string url)
	{
		var html = await _http.GetAsync(url);
		var content = html.Content.ReadAsStreamAsync();

		var browser = BrowsingContext.New(Configuration.Default);

		async void Request(VirtualResponse r) => r.Content(await content);
		var dom = await browser.OpenAsync(Request);

		var rows = dom.DocumentElement.QuerySelectorAll("tbody tr");
		var records = new List<Record>();
		foreach (var row in rows)
		{
			var cells = row.QuerySelectorAll("td");

			var day = cells[0].TextContent;
			if (string.IsNullOrWhiteSpace(day) || !DateTime.TryParse(day, out _))
				continue;

			records.Add(new Record(cells.ToList()));
		}

		return records;
	}
}
